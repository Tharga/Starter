using Quilt4Net.Toolkit;
using Quilt4Net.Toolkit.Features.Health;
#if (IncludeQuilt4Net)
using Quilt4Net.Toolkit.Framework;
#endif
using Tharga.MongoDB;

namespace Tharga.Blazor1.Framework;

// Health checks for the application.
// See https://github.com/Quilt4/Quilt4Net.Toolkit for documentation.
internal class ComponentService : IComponentService
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDbServiceFactory _mongoDbServiceFactory;
#if (IncludeQuilt4Net)
    private readonly IConnectionService _connectionService;
#endif

    public ComponentService(
        IConfiguration configuration,
        IMongoDbServiceFactory mongoDbServiceFactory
#if (IncludeQuilt4Net)
        , IConnectionService connectionService
#endif
        )
    {
        _configuration = configuration;
        _mongoDbServiceFactory = mongoDbServiceFactory;
#if (IncludeQuilt4Net)
        _connectionService = connectionService;
#endif
    }

    public IEnumerable<Component> GetComponents()
    {
        // MongoDB connectivity check for each connection string.
        var connectionStrings = _configuration.GetSection("ConnectionStrings");
        foreach (var connectionString in connectionStrings.GetChildren())
        {
            yield return new Component
            {
                Name = $"Database.{connectionString.Key}",
                Essential = true,
                CheckAsync = async _ =>
                {
                    var service = _mongoDbServiceFactory.GetMongoDbService(() => new DatabaseContext { ConfigurationName = connectionString.Key });
                    var info = await service.GetInfoAsync();

                    return new CheckResult
                    {
                        Success = info.CanConnect,
                        Message = info.Message
                    };
                }
            };
        }

#if (IncludeQuilt4Net)
        // Quilt4Net service connectivity checks.
        foreach (var service in Enum.GetValues<Service>())
        {
            yield return new Component
            {
                Name = $"Quilt4Net.{service}",
                CheckAsync = async _ =>
                {
                    var response = await _connectionService.CanConnectAsync(service);
                    return new CheckResult
                    {
                        Success = response.Success,
                        Message = $"{response.Message} @{response.Address}"
                    };
                }
            };
        }
#endif
    }
}
