using System.Security.Claims;
using Tharga.Team;
using Tharga.Team.Blazor.Framework;

namespace Tharga.Blazor1.Framework;

/// <summary>
/// Grants the Developer role to the e-mail addresses listed under <c>Developers:Emails</c> in configuration.
/// The Developer role opens the Developer menu (database, users, audit, ...) and maps to the system scopes
/// configured in <c>Program.cs</c>. Keep the list in user secrets or app settings, never in the repository:
/// <c>dotnet user-secrets set "Developers:Emails:0" "you@example.com"</c>.
/// </summary>
public sealed class DeveloperRoleEnricher(IConfiguration configuration) : ITeamClaimsEnricher
{
    public const string ConfigurationKey = "Developers:Emails";

    public Task EnrichAsync(ClaimsIdentity identity)
    {
        var developers = configuration.GetSection(ConfigurationKey).Get<string[]>() ?? [];
        var email = identity.FindFirst("preferred_username")?.Value ?? identity.FindFirst(ClaimTypes.Email)?.Value;

        if (email != null
            && developers.Contains(email, StringComparer.OrdinalIgnoreCase)
            && !identity.HasClaim(ClaimTypes.Role, Roles.Developer))
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, Roles.Developer));
        }

        return Task.CompletedTask;
    }
}
