using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Tharga.Team.MongoDB;

namespace Tharga.Blazor1.Features.Team;

public class AppUserService(
    AuthenticationStateProvider authenticationStateProvider,
    IUserRepository<UserEntity> userRepository)
    : UserServiceRepositoryBase<UserEntity>(authenticationStateProvider, userRepository)
{
    protected override Task<UserEntity> CreateUserEntityAsync(ClaimsPrincipal claimsPrincipal, string identity)
    {
        var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email) ?? "";
        var name = claimsPrincipal.FindFirstValue(ClaimTypes.Name);

        return Task.FromResult(new UserEntity
        {
            Key = identity,
            Identity = identity,
            EMail = email,
            Name = name
        });
    }
}
