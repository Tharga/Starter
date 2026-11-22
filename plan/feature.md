# Feature: blazor-platform defaults and options from real projects

Stacked on PR #4 (`feature/update-blazor-platform-packages`). Follow-up to
https://github.com/Tharga/Starter/issues/3.

## Goal
A project generated from `blazor-platform` starts with the Tharga.Team configuration that the existing
Tharga projects have converged on, instead of the bare minimum. Based on a survey of five existing Tharga applications;
counts below are "applications out of five".

## Originating branch
`feature/update-blazor-platform-packages` (PR #4). Rebase onto `develop` once PR #4 is merged.

## Scope

### Defaults (set by 4 or 5 of 5)
1. Audit storage `Logger | MongoDB` (5 of 5 include MongoDB; the library default is Logger only, which leaves the audit pages empty).
2. `ApiKey.AdvancedMode = true` (5), `Controllers.SwaggerTitle` (3).
3. Simulation: `Blazor.Simulation.Enabled = true` (4) with the simulation bar/indicator in the layout.
4. Consent: `ShowToggle = true` (4), `AccessLevel = Administrator` (4). `GrantTeamsRead` stays off (3 of 5, cross-team visibility, one project omits it on purpose); a comment explains it.
5. Developer system role: map `Roles.Developer` to `SystemUserScopes.Manage`, `AuditScopes.Read` (5), `ApiKeyScopes.SystemManage`, `SystemTeamScopes.Delete` (4).
6. A claims enricher that grants the Developer role from `Developers:Emails` in config (a pattern one of the surveyed applications uses). The template's `/developer` pages require the Developer role but nothing grants it today.
7. Component parameters: `ApiKeyView ShowRoles/ShowScopeOverrides/ShowAuditLogButton` (5/5/4), a Developer `SystemApiKeyView` page (4), `UsersView ShowAuditLogButton`.
8. Example scopes in the Viewer / User / Administrator pattern with descriptions and a group, as commented hints.

### New template options
- `IncludeMcp` (default true; 5 of 5): `Tharga.Team.Mcp`, `AddThargaMcp(mcp => mcp.AddTeam())`, `UseThargaMcp()`, `mcp:discover` for Developer.
- `IncludeEntra` (default true; 4 of 5): `Tharga.Team.Entra`, `AddThargaEntraUserDirectory` only when `AzureAd:ClientSecret` is set, `WriteNameToDirectory`.
- `IncludeDynamicRoles` (default false; 3 of 5): `EnableDynamicRoles = true` and a Roles page with `TenantRoleManager`.

## Out of scope
- Support, Images, Email, sign-in methods, sessions, claim revalidation, custom entity fields (used by 0 to 2 projects).
- Changes in the other projects (read-only survey; findings reported to the user).

## Acceptance criteria
- [ ] All three new options exist in `template.json`, work on and off, and generated projects build for on/off combinations.
- [ ] Defaults above are in the generated `Program.cs` / pages.
- [ ] Generated app starts (DI validation) with the defaults, or the gap is stated.
- [ ] Tests updated and extended; full suite passes (run one class at a time).
- [ ] README documents the options and the Developer role config.

## Done condition
All criteria met, tests pass, user confirms.
