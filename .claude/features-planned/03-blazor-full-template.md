# Feature: tharga-blazor-full template

## Goal
Create a `tharga-blazor-full` template that includes all 14 features — the complete Tharga platform experience.

## Scope
- New template under `templates/blazor-full/` based on `templates/blazor-platform/`
- Feature 10: Rate Limiting (built-in, always included)
- Feature 11: Scopes via `AddThargaScopes()`
- Feature 12: Team Roles via `AddThargaTenantRoles()`
- Feature 13: Audit Logging via `AddThargaAuditLogging()`
- Feature 14: Health Endpoints via `Quilt4Net.Toolkit.Health` (always included)
- Optional flag: `--IncludeSamples` (default true)
- All developer pages included
- Update Tharga.Starter.csproj to include new template
- Integration tests for the new template

## Depends on
- Feature 02 (blazor-platform template)

## Acceptance criteria
- Template installs and generates a project that builds
- All 14 features are wired up
- Scopes, roles, and audit logging are configured
- Integration tests pass
- README feature table updated with new template column
