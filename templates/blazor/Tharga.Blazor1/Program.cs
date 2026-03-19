using Tharga.Blazor1.Components;
using Tharga.Blazor1.Framework;
using Radzen;
using Tharga.Blazor.Framework;

namespace Tharga.Blazor1;

public class Program
{
    public static void Main(string[] args)
    {
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
        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

        app.Run();
    }
}
