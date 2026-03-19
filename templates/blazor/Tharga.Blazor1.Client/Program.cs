using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Tharga.Blazor1.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        await builder.Build().RunAsync();
    }
}
