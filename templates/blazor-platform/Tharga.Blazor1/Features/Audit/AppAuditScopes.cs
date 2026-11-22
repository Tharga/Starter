namespace Tharga.Blazor1.Features.Audit;

/// <summary>
/// Application-specific audit scopes. The audit-read scope itself (<c>AuditScopes.Read</c>) comes from Tharga.Team.
/// </summary>
public static class AppAuditScopes
{
    public const string ApiKeyUsage = "audit:apikey";
}
