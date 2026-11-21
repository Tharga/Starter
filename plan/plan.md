# Plan: Update blazor-platform template to current package versions

## Steps

### 1. Baseline
- [x] Test baseline on `develop` (2026-11-21): **22 tests, all pass** (Console 6, PackAndInstall 4, Blazor 4, BlazorPlatform 8), ~3.5 min.
  - The original plan said 47 tests; the real count is 22.
  - **Run the classes one at a time.** The full-solution run in parallel hung for 46+ min (classes share the global `dotnet new` template store and NuGet). Run per class with `--filter "FullyQualifiedName~.<Class>"` and log to a file; never pipe `dotnet test` through `tail`.
- [x] NU190x before (default `blazor-platform` project, clean restore): 7 vulnerable packages, all transitive except the first:
  - Microsoft.AspNetCore.Components.WebAssembly.Server 10.0.6 (High, top-level)
  - Microsoft.AspNetCore.DataProtection 10.0.0 (**Critical**)
  - Microsoft.OpenApi 2.4.1 (High)
  - SharpCompress 0.30.1 (Moderate), Snappier 1.0.0 (High)
  - System.Security.Cryptography.Pkcs 10.0.0 (High), System.Security.Cryptography.Xml 10.0.0 (High, many advisories)
  - Reported in the `IntegrationTests` project; `Tests` shows SharpCompress.

### 2. Research the majors
- [x] Latest stable on nuget.org (2026-11-21): Tharga.Team.* 4.8.1, Tharga.MongoDB.Blazor 2.21.1, Tharga.Toolkit.Standard 1.19.0, Tharga.Cache.Blazor 1.2.1, Quilt4Net.Toolkit.* 1.0.32, Tharga.Blazor 2.5.1, Tharga.Console 4.4.1, Radzen.Blazor 13.2.1, Microsoft.AspNetCore.* 10.0.15 (11.0.0 exists but templates target net10.0, so stay on 10.0.x)
- [~] Breaking changes that touch the template (reference: `Toolkit/Team/Tharga.Team.Sample/Program.cs`, `docs/articles/getting-started.md`):
  - Team 4.x: `builder.AddThargaTeam(o => ...)` + `app.UseThargaTeam()` replace AddThargaAuth / AddThargaControllers / AddThargaTeamBlazor / AddThargaApiKeys / AddThargaScopes / AddThargaTenantRoles / AddThargaAuditLogging and UseThargaAuth / UseThargaControllers. Scopes/roles go in `o.ConfigureScopes` / `o.ConfigureTenantRoles`. Verify against the 4.8.1 package, not only the Sample.
  - Quilt4Net 1.x: `AddQuilt4NetApiLogging` is obsolete, use `AddQuilt4NetLogging().AddHttpRequestLogging()` + `app.UseQuilt4NetLogging()`.
  - Open: Radzen 10 to 13 and Tharga.Console 3.* to 4.x are major bumps, awaiting the user's decision.

### 3. Bump versions
- [ ] Update `.csproj` files (blazor-platform first, then blazor and console)
- [ ] Build the generated project; fix compile errors

### 4. Adapt template code
- [ ] Move to the current Tharga.Team entry point; remove hand-wired registrations that are now redundant
- [ ] Quilt4Net 1.x registrations; add `AddQuilt4NetContent()` (issue #2)
- [ ] Add a test that guards #2 (template with IncludeQuilt4Net contains the content registration)

### 5. Verify
- [ ] Full test suite + template integration tests; compare test count with baseline
- [ ] `dotnet new` + build for: defaults, each of IncludeQuilt4Net / IncludeHealth / IncludeCache on and off
- [ ] Clean restore: NU190x warnings gone, or documented

### 6. Docs
- [ ] Update template README (and root README if it lists versions)

### 7. Finish
- [ ] Commit at milestones (conventional commits), full test run before each commit
- [ ] Summarize; user tests; closing only on user's word

## Notes
- Public repo: no internal project names or tracker numbers beyond the GitHub issue in commit/PR text.

## Last session
Branch `feature/update-blazor-platform-packages` created from `develop`. Plan drafted, awaiting user approval before any code changes.
