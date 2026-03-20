using Quilt4Net.Toolkit;
using Quilt4Net.Toolkit.Features.Health;

namespace Tharga.Blazor1.Framework;

// Sample IComponentService — add your own health checks here.
// See https://github.com/Quilt4/Quilt4Net.Toolkit for documentation.
internal class ComponentService : IComponentService
{
    public IEnumerable<Component> GetComponents()
    {
        yield return new Component
        {
            Name = "sample-check",
            Essential = false,
            CheckAsync = async _ =>
            {
                await Task.CompletedTask;
                return new CheckResult
                {
                    Success = true,
                    Message = "Sample component is healthy"
                };
            }
        };
    }
}
