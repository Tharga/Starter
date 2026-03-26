using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Tharga.Blazor1.Features.Team;

public class TeamCookieClaimsTransformation : IClaimsTransformation
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TeamCookieClaimsTransformation(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
            return Task.FromResult(principal);

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return Task.FromResult(principal);

        var teamKey = httpContext.Request.Cookies["selected_team_id"];
        if (!string.IsNullOrEmpty(teamKey) && !principal.HasClaim("team_id", teamKey))
        {
            identity.AddClaim(new Claim("team_id", teamKey));
        }

        return Task.FromResult(principal);
    }
}
