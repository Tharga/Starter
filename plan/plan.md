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
- [x] Updated `.csproj` files in blazor-platform, blazor and console to the latest stable that works on .NET 10 (user decision: highest possible, keep .NET 10; Microsoft.AspNetCore.* on 10.0.15, not 11.0.0; Radzen 13.2.1; Tharga.Console 4.*).
- [x] Tharga.Console 4.x supports only net10.0/net11.0 (NU1202 on net9.0), so the **console template and its test project moved from net9.0 to net10.0**. `Tharga.Starter.Tests` (test host, not a template) stays on net9.0; CI already installs the 9.0 and 10.0 SDKs.

### 4. Adapt template code
- [x] `Program.cs` uses `AddThargaTeam(...)` / `UseThargaTeam()`; scopes, tenant roles and audit moved into its options. Explicit UseRouting/UseAuthentication/UseAuthorization kept.
- [x] Template's `AuditScopes` clashed with `Tharga.Team.AuditScopes`; renamed to `AppAuditScopes` (only `ApiKeyUsage`), the toolkit's `AuditScopes.Read` is used.
- [x] `ShowMemberRoles` option removed in Team 4.x; now `ShowRoles="true"` on `TeamComponent` in `Teams.razor`. `ShowScopeOverrides` was false (the default), so unset.
- [x] Quilt4Net 1.x: `AddQuilt4NetApiLogging` (obsolete) replaced by `AddQuilt4NetLogging().AddHttpRequestLogging()`, plus `app.UseQuilt4NetLogging()` (the old setup never added the middleware).
- [x] Issue #2: no `AddQuilt4NetContent()` added. In Quilt4Net.Toolkit 1.0.32 `AddQuilt4NetBlazorContent` calls `AddQuilt4NetContent` itself, so the upgrade fixes it. **Not verified at runtime** (needs a running app with `Quilt4Net:ApiKey` only); guarded by a template test that checks the registration is present.
- [x] Test added: `IncludeQuilt4Net_RegistersContentAndUsesCurrentLogging`. Updated `DefaultOptions_ContainsExpectedStructureAndContent` to the new entry points.

### 5. Verify
- [x] Full suite, run one class at a time (2026-11-21): **23 tests, all pass** (Console 6, PackAndInstall 4, Blazor 4, BlazorPlatform 9). Baseline was 22; +1 new test.
- [x] Default `blazor-platform` project builds with 0 errors, 0 warnings, 0 NU190x (including its Tests and IntegrationTests projects).
- [ ] Explicit `dotnet list package --vulnerable --include-transitive` after the upgrade (not rerun; the clean build above shows no NU190x warnings).
- [ ] Not covered by a test: IncludeHealth=false alone and IncludeRateLimiting on/off individually (defaults, AllOptionsDisabled, NoSamples and IncludeCache are covered).
- [ ] Runtime check of the generated app (Radzen 10 to 13 and Team 2 to 4 UI changes are compile-checked only). Needs the user to run it.

### 6. Docs
- [x] `templates/blazor-platform/README.md`: added how the features are registered (`AddThargaTeam`) and the starting package versions. Root README had nothing version-specific.

### 7. Finish
- [x] Full test run before commit (23 pass). Commit with conventional message, no attribution lines (user instruction).
- [~] Push and open PR to `develop` (user approved push and PR).
- [ ] User tests the generated app; closing (#3, #2, archive plan) only on the user's word

## Notes
- Public repo: no internal project names or tracker numbers beyond the GitHub issue in commit/PR text.

## Last session
2026-11-21: Baseline recorded, all templates bumped to latest stable on .NET 10, `blazor-platform` moved to `AddThargaTeam`/`UseThargaTeam` and Quilt4Net 1.x logging, console template moved to net10.0, tests updated (+1), README updated. 23 tests pass. PR opened to `develop`.

Next: the user runs a generated project and checks the UI (Radzen 13, Team 4) and that `Quilt4Net:ApiKey` alone no longer breaks the content connectivity check (#2). Then close #3 and #2 and archive this plan to `plans/Starter/done/` on the user's word. README root/docs need no further change.
