# Plan: Template Fixes (Blazor Platform)

## Steps

### 1. Fix ContentAdmin namespace in Content.razor
- [x] Change `Quilt4Net.Toolkit.Blazor.Features.Content.ContentAdmin` to `Quilt4Net.Toolkit.Blazor.Features.Language.ContentAdmin`

### 2. Register audit scopes in AddThargaScopes
- [x] Add `using Tharga.Blazor1.Features.Audit;` to Program.cs
- [x] Register `AuditScopes.Read` and `AuditScopes.ApiKeyUsage` at `AccessLevel.User`

### 3. Fix claim extraction in AppUserService
- [x] Add `Tharga.Toolkit.Standard` v1.15.20 to csproj
- [x] Replace FindFirstValue with `GetEmail()` and `GetDisplayName()`

### 4. Fix CreateTeam in AppTeamService
- [x] Set `Name = displayName` on the owner TeamMember

### 5. Add LanguageSelector to NavMenu
- [x] Add `@using Quilt4Net.Toolkit.Blazor.Features.Language` to `_Imports.razor` (conditional)
- [x] Add `<LanguageSelector />` to NavMenu header (conditional on IncludeQuilt4Net)

### 6. Update tests
- [x] 7 new tests added (namespace, scopes, claims, displayName, LanguageSelector with/without Quilt4Net)
- [x] Fixed test parallelism issue with `[Collection("Template")]`
- [x] All 47 tests pass

### 7. Check for NuGet package updates
- [x] Updated blazor-platform: Team.Blazor 2.0.5, Team.Service 2.0.5, Team.MongoDB 2.0.6, Quilt4Net 0.6.9
- [x] Updated blazor: Quilt4Net.Toolkit.Health 0.6.9
- [x] No breaking changes in any updates

### 8. Mark requests as Done in Requests.md
- [x] All 5 requests marked Done with summaries
- [x] Follow-up entry added for Florida

## Last session
All steps complete. 47/47 tests pass. Ready for user review and commit.
