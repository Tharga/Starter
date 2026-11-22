# Plan: blazor-platform defaults and options from real projects

Stacked on PR #4 (`feature/update-blazor-platform-packages`).

## Steps

### 1. Survey (read-only)
- [x] Five existing Tharga applications surveyed: options, scopes, roles, component parameters. Findings are in `feature.md` as counts out of five.
- [x] User chose: defaults + new options (IncludeMcp, IncludeEntra, IncludeDynamicRoles), on a new branch stacked on PR #4.

### 2. Defaults
- [x] `Program.cs`: audit Logger | MongoDB, `ApiKey.AdvancedMode`, `Controllers.SwaggerTitle`, `Simulation.Enabled`, `Consent.ShowToggle` / `AccessLevel`, `ConfigureSystemRoles` (Developer), `AddClaimsEnricher<DeveloperRoleEnricher>`. `GrantTeamsRead` left off with a comment.
- [x] `Framework/DeveloperRoleEnricher.cs` grants Developer from `Developers:Emails` (a pattern one of the surveyed applications uses); `appsettings.json` has an empty `Developers:Emails`.
- [x] Pages: `ApiKeyView` with `ShowRoles` / `ShowScopeOverrides` / `ShowAuditLogButton`, `UsersView ShowAuditLogButton`, new Developer page `SystemApiKeys.razor` with a menu entry, `AccessSimulationBar` in the header.
- [x] Scope and role examples as commented hints (Viewer / User / Administrator, with descriptions and a group).

### 3. Options
- [x] `IncludeMcp` (default true): `Tharga.Team.Mcp`, `AddThargaMcp(mcp.AddTeam())`, `UseThargaMcp()`, `mcp:discover` for Developer.
- [x] `IncludeEntra` (default true): `Tharga.Team.Entra`, `AddThargaEntraUserDirectory` only when `AzureAd:ClientSecret` is set, `WriteNameToDirectory`.
- [x] `IncludeDynamicRoles` (default false): `EnableDynamicRoles`, `Components/Pages/CustomRoles.razor` (route `/roles`) with `TenantRoleManager`, menu entry.

### 4. Problems found on the way
- [x] **Startup crash (also in PR #4):** Tharga.Team 4.x registers `audit:read` itself and the template registered it again ("Scope 'audit:read' is already registered"). It compiled and passed the old tests because they only built the project. Removed here. **PR #4 still needs the same one-line fix.**
- [x] `AccessSimulationBar` rendered as an unknown HTML element (RZ10012 warning) until `@using Tharga.Team.Blazor.Features.Simulation` was added.
- [x] A page named `Roles.razor` generates a class `Roles` that shadows the toolkit's `Roles` constants inside the page (CS0117). Renamed `CustomRoles.razor`; found by the new startup test, because the page only exists with `IncludeDynamicRoles`.
- [x] Test helper: build tests took 15 min each instead of 13 s. Cause: MSBuild worker nodes inherit the redirected output pipe and live 15 min idle, and the helper reads the output to the end. Fixed by disabling node reuse, the shared compiler and the MSBuild server for the processes the tests start. Class time went from 15+ min per test to 3.5 min total.

### 5. Tests
- [x] New: `DefaultOptions_ContainsProjectDefaults`, `DefaultOptions_StartsUp` (runs the generated app's own integration tests with placeholder config), `McpAndEntraDisabled_LeaveNoTraces_AndBuild`, `IncludeDynamicRoles_AddsRolesPage_AndStartsUp`.
- [~] Full suite one class at a time: platform class 12 of 13 before the `CustomRoles` rename; full rerun in progress.

### 6. Docs
- [x] Template README: options table, what is configured out of the box, `Developers:Emails`.

### 7. Finish
- [ ] Commit, push, open PR targeting `feature/update-blazor-platform-packages` (retarget to `develop` after PR #4 merges).
- [ ] The `audit:read` fix on PR #4's branch (separate commit, ask before pushing).
- [ ] Runtime check by the user: sign in, set `Developers:Emails`, check the Developer menu, system API keys, simulation, MCP endpoint. Not done by me: needs a real identity provider and MongoDB.

## Not verified
- The app has been started only by the generated integration tests (home page) with placeholder config. Sign-in, the Developer role grant, simulation, MCP calls and the Entra directory have not been exercised.
- `Consent.AccessLevel`, `ShowToggle` and the system role mapping are compile-checked and DI-validated only.

## Last session
2026-11-22: Feature built, tests extended, README written. Waiting for the full suite, then commit/push/PR.
