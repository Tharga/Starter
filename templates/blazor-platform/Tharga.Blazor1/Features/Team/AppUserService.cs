using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Tharga.Team.MongoDB;
using Tharga.Toolkit;

namespace Tharga.Blazor1.Features.Team;

public class AppUserService(
    AuthenticationStateProvider authenticationStateProvider,
    IUserRepository<UserEntity> userRepository)
    : UserServiceRepositoryBase<UserEntity>(authenticationStateProvider, userRepository)
{
    protected override Task<UserEntity> CreateUserEntityAsync(ClaimsPrincipal claimsPrincipal, string identity)
    {
        var email = claimsPrincipal.GetEmail() ?? "unknown";
        var name = claimsPrincipal.GetDisplayName();

        return Task.FromResult(new UserEntity
        {
            Key = identity,
            Identity = identity,
            EMail = email,
            Name = name
        });
    }
}
