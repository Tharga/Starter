using Tharga.Blazor1.Components;
using Tharga.Blazor1.Features.Audit;
using Tharga.Blazor1.Features.Team;
using Tharga.Blazor1.Framework;
using Radzen;
using Tharga.MongoDB;
using Tharga.Team;
using Tharga.Team.Blazor.Features.Authentication;
using Tharga.Team.Blazor.Framework;
using Tharga.Team.MongoDB;
using Tharga.Team.Service;
using Tharga.Team.Service.Audit;
#if (IncludeCache)
using Tharga.Cache;
#endif
#if (IncludeMcp)
using Tharga.Mcp;
using Tharga.Team.Mcp;
#endif
#if (IncludeEntra)
using Tharga.Team.Entra;
#endif
#if (IncludeHealth)
using Quilt4Net.Toolkit.Health;
#endif
#if (IncludeQuilt4Net)
using Quilt4Net.Toolkit;
using Quilt4Net.Toolkit.Api;
using Quilt4Net.Toolkit.Blazor;
#endif
#if (IncludeRateLimiting)
using System.Threading.RateLimiting;
#endif

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Step 1: UI Foundation
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = Constants.ThemeStorageName;
    options.Duration = TimeSpan.FromDays(365);
});

// Step 2: Tharga Team — one call registers authentication, API key authentication, API controllers
// & Swagger, the team management UI, scopes, tenant roles and audit logging.
// See https://github.com/Tharga/Team for documentation.
builder.AddThargaTeam(o =>
{
    o.Blazor.Title = "Tharga.Blazor1";
    o.Blazor.SkipAuthStateDecoration = true;
    o.Blazor.RegisterTeamService<AppTeamService, AppUserService, TeamMember>();
    o.Blazor.RegisterApiKeyAdministrationService<ApiKeyAdministrationService>();

    // Grants the Developer role to the addresses in Developers:Emails (see DeveloperRoleEnricher).
    o.Blazor.AddClaimsEnricher<DeveloperRoleEnricher>();

    // Let a team administrator view the app as a less privileged member (off by default in the library).
    o.Blazor.Simulation.Enabled = true;

    // Teams can consent to let Developers in to help; the toggle is shown on the team page.
    o.Blazor.Consent.ShowToggle = true;
    o.Blazor.Consent.AccessLevel = AccessLevel.Administrator;
    // Consent.GrantTeamsRead = true would let Developers see every team (not just consenting ones). It is
    // off here on purpose; turn it on if your Developers need cross-team visibility.

    // Advanced mode unlocks the full API key UI (access level, roles, scope overrides, tags).
    o.ApiKey.AdvancedMode = true;
    o.Controllers.SwaggerTitle = "Tharga.Blazor1 API";
#if (IncludeEntra)
    // The rename dialog offers to update the name in Entra too (needs User.ReadWrite.All).
    o.Blazor.WriteNameToDirectory = true;
#endif
#if (IncludeDynamicRoles)
    // Team administrators can define their own roles at runtime on the Roles page.
    o.EnableDynamicRoles = true;
#endif

    // Scopes: Viewer reads, User writes, Administrator manages. Always give a description.
    o.ConfigureScopes = scopes =>
    {
        // audit:read is registered by Tharga.Team itself; registering it again throws at startup.
        scopes.Register(AppAuditScopes.ApiKeyUsage, AccessLevel.User);
        // Register application-specific scopes here, grouped by product area:
        // scopes.DefineGroup("orders", "Orders");
        // scopes.Register("orders:read", AccessLevel.Viewer, "View orders.", group: "orders");
        // scopes.Register("orders:write", AccessLevel.User, "Create and edit orders.", group: "orders");
        // scopes.Register("orders:manage", AccessLevel.Administrator, "Refund and delete orders.", group: "orders");
    };

    // Tenant roles: named bundles of scopes that are assigned to team members.
    o.ConfigureTenantRoles = roles =>
    {
        roles.Register("TeamDeveloper", new[] { ApiKeyScopes.Manage });
        // Register application-specific roles here:
        // roles.Register("Editor", new[] { "orders:read", "orders:write" }, "Edits orders.");
    };

    // System roles: what the Developer role may do across all teams.
    o.ConfigureSystemRoles = roles =>
    {
        roles.Map(Roles.Developer,
            SystemUserScopes.Manage,
            AuditScopes.Read,
            ApiKeyScopes.SystemManage,
            SystemTeamScopes.Delete
#if (IncludeMcp)
            , "mcp:discover"
#endif
        );
    };

    // Audit entries are both logged and stored in MongoDB, so the audit pages have something to show.
    o.Audit = new AuditOptions { StorageMode = AuditStorageMode.Logger | AuditStorageMode.MongoDB };
});
builder.Services.AddThargaTeamRepository(o =>
{
    o.RegisterUserRepository<UserEntity>();
    o.RegisterTeamRepository<TeamEntity, TeamMember>();
});

// MongoDB
builder.AddMongoDB();

#if (IncludeEntra)
// Entra as the user directory (verify users, list directory-only users, opt-in directory delete).
// App-only Graph auth needs a client secret, so the directory only lights up when one is configured:
//   dotnet user-secrets set "AzureAd:ClientSecret" "<secret>"
// Graph permissions: User.Read.All (verify/list), User.ReadWrite.All (delete, rename).
if (!string.IsNullOrEmpty(builder.Configuration["AzureAd:ClientSecret"]))
{
    builder.Services.AddThargaEntraUserDirectory(builder.Configuration);
}
#endif

#if (IncludeMcp)
// MCP endpoint: lets agents use the app with an API key. AddTeam() contributes the team-aware
// authentication and the mcp:discover scope. Add your own tools with mcp.AddToolProvider<T>().
builder.Services.AddThargaMcp(mcp =>
{
    mcp.AddTeam();
});
#endif

#if (IncludeCache)
// Tharga.Cache — see https://github.com/Tharga/Cache for documentation.
builder.Services.AddCache();
#endif

#if (IncludeQuilt4Net)
// Quilt4Net: logging (incl. HTTP request logging), remote configuration, and content management.
// See https://github.com/Quilt4/Quilt4Net.Toolkit for documentation.
builder.AddQuilt4NetLogging()
    .AddHttpRequestLogging();
builder.AddQuilt4NetRemoteConfiguration();
builder.AddQuilt4NetBlazorContent();
#endif

#if (IncludeHealth)
builder.AddQuilt4NetHealth(o =>
{
    o.AddComponentService<ComponentService>();
});
#endif
#if (IncludeRateLimiting)
// Rate limiting: 100 requests per minute per IP address.
// Adjust PermitLimit and Window to match your traffic requirements.
// See https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit for options.
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
#endif

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
#if (IncludeQuilt4Net)
app.UseQuilt4NetLogging();
#endif
#if (IncludeRateLimiting)
app.UseRateLimiter();
#endif

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Tharga Team middleware (authentication endpoints, controllers, Swagger)
app.UseThargaTeam();
#if (IncludeMcp)
app.UseThargaMcp();
#endif

#if (IncludeHealth)
app.UseQuilt4NetHealth();
#endif

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Tharga.Blazor1.Client._Imports).Assembly);

app.Run();

public partial class Program;
