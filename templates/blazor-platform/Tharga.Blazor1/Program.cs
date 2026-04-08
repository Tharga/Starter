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

// Step 2: Authentication
builder.AddThargaAuth();

// Step 3: API Controllers & Swagger
builder.Services.AddThargaControllers();

// Step 4: Team Management
builder.Services.AddThargaTeamBlazor(o =>
{
    o.Title = "Tharga.Blazor1";
    o.SkipAuthStateDecoration = true;
    o.RegisterTeamService<AppTeamService, AppUserService, TeamMember>();
    o.RegisterApiKeyAdministrationService<ApiKeyAdministrationService>();
    o.ShowMemberRoles = true;
    o.ShowScopeOverrides = false;
});
builder.Services.AddThargaTeamRepository(o =>
{
    o.RegisterUserRepository<UserEntity>();
    o.RegisterTeamRepository<TeamEntity, TeamMember>();
});

// MongoDB
builder.AddMongoDB();

// Step 5: API Key Authentication
builder.Services.AddThargaApiKeys();
builder.Services.AddAuthentication()
    .AddThargaApiKeyAuthentication();

// Step 6: Scopes
builder.Services.AddThargaScopes(scopes =>
{
    scopes.Register(AuditScopes.Read, AccessLevel.User);
    scopes.Register(AuditScopes.ApiKeyUsage, AccessLevel.User);
    // Register application-specific scopes here:
    // scopes.Register("feature:read", AccessLevel.Viewer);
    // scopes.Register("feature:write", AccessLevel.User);
});

// Step 7: Tenant Roles
builder.Services.AddThargaTenantRoles(roles =>
{
    roles.Register("TeamDeveloper", new[] { ApiKeyScopes.Manage });
    // Register application-specific roles here:
    // roles.Register("Editor", new[] { "feature:read", "feature:write" });
});

// Step 8: Audit Logging
builder.Services.AddThargaAuditLogging();

#if (IncludeQuilt4Net)
// Quilt4Net: API logging, remote configuration, and content management.
// See https://github.com/Quilt4/Quilt4Net.Toolkit for documentation.
builder.AddQuilt4NetApiLogging();
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
#if (IncludeRateLimiting)
app.UseRateLimiter();
#endif

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Auth & Controllers middleware
app.UseThargaAuth();
app.UseThargaControllers();

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
