using Tharga.MongoDB;
using Tharga.Team;

namespace Tharga.Blazor1.Features.Team;

public record UserEntity : EntityBase, IUser
{
    public string Key { get; init; } = "";
    public string Identity { get; init; } = "";
    public string EMail { get; init; } = "";
    public string Name { get; init; }
}
