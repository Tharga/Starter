using Tharga.Blazor1.Components;
using Tharga.Blazor1.Framework;
using Radzen;
using Tharga.Blazor.Framework;
#if (IncludeHealth)
using Quilt4Net.Toolkit.Health;
#endif
#if (IncludeRateLimiting)
using System.Threading.RateLimiting;
#endif

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = Constants.ThemeStorageName;
    options.Duration = TimeSpan.FromDays(365);
});

builder.Services.AddThargaBlazor(o =>
{
    o.Title = "Tharga.Blazor1";
});

#if (IncludeHealth)
builder.AddQuilt4NetHealth(o =>
{
#if (IncludeSamples)
    // Sample component service — see https://github.com/Quilt4/Quilt4Net.Toolkit for documentation.
    o.AddComponentService<ComponentService>();
#endif
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
app.UseAntiforgery();
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
