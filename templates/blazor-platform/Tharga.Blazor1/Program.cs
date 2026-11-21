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

    // Scopes
    o.ConfigureScopes = scopes =>
    {
        scopes.Register(AuditScopes.Read, AccessLevel.User);
        scopes.Register(AppAuditScopes.ApiKeyUsage, AccessLevel.User);
        // Register application-specific scopes here:
        // scopes.Register("feature:read", AccessLevel.Viewer);
        // scopes.Register("feature:write", AccessLevel.User);
    };

    // Tenant roles
    o.ConfigureTenantRoles = roles =>
    {
        roles.Register("TeamDeveloper", new[] { ApiKeyScopes.Manage });
        // Register application-specific roles here:
        // roles.Register("Editor", new[] { "feature:read", "feature:write" });
    };

    // Audit logging
    o.Audit = new AuditOptions();
});
builder.Services.AddThargaTeamRepository(o =>
{
    o.RegisterUserRepository<UserEntity>();
    o.RegisterTeamRepository<TeamEntity, TeamMember>();
});

// MongoDB
builder.AddMongoDB();

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
