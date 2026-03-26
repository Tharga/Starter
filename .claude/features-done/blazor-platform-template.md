# Feature: Blazor Platform Template

## Goal
Create a new `tharga-blazor-platform` template that includes the full Tharga Platform stack: UI foundation, authentication, API controllers, team management, API keys, scopes, tenant roles, and audit logging.

## Originating branch
develop

## Scope
- New template under `templates/blazor-platform/` based on `templates/blazor/`
- Step 2: Authentication via `Tharga.Team.Blazor` (`AddThargaAuth()` / `UseThargaAuth()`)
- Step 3: API Controllers & Swagger via `Tharga.Team.Service` (`AddThargaControllers()` / `UseThargaControllers()`)
- Step 4: Team Management via `Tharga.Team.MongoDB` (entities, services, repository)
- Step 5: API Key Authentication via `AddThargaApiKeys()` / `AddThargaApiKeyAuthentication()`
- Step 6: Scopes via `AddThargaScopes()`
- Step 7: Tenant Roles via `AddThargaTenantRoles()`
- Step 8: Audit Logging via `AddThargaAuditLogging()`
- Optional flags: `--IncludeHealth`, `--IncludeRateLimiting`
- Placeholder config in appsettings.json (AzureAd, ConnectionStrings)
- Developer/admin pages (Profile, Team, Users, ApiKeys, AuditLog)
- Integration tests for the new template

## Acceptance criteria
- [ ] Template installs and generates a project that builds
- [ ] Auth, Controllers, Teams, API Keys, Scopes, Roles, Audit are all wired up
- [ ] Placeholder config values in appsettings.json with clear comments
- [ ] Optional flags (Health, RateLimiting) work correctly
- [ ] Integration tests pass
- [ ] Starter solution tests pass
- [ ] README updated with new template

## Done condition
User confirms the template works correctly
