# Plan: Update all NuGet packages

## Steps

### 1. Review changelogs for big jumps
- [x] Tharga.Team 2.0.5 → 2.0.10: Minor fixes only (Team/User monitor tracking, AccessLevelProxy audit). No API changes.
- [x] Quilt4Net.Toolkit 0.6.9 → 0.6.16: Safe. `AddQuilt4NetApiLogging()` becomes `[Obsolete]` — produces warning but works.
- [x] Tharga.MongoDB.Blazor 2.10.0 → 2.10.3: Safe. New `TrackMongoCollection` API (additive) + firewall fix.

### 2. Update Tharga packages
- [x] blazor server: Tharga.Blazor 2.1.2 → 2.1.3
- [x] blazor-platform server: Team.Blazor/Service 2.0.5 → 2.0.10, Team.MongoDB 2.0.6 → 2.0.10, Toolkit.Standard 1.15.20 → 1.15.21, MongoDB.Blazor 2.10.0 → 2.10.3

### 3. Update Quilt4Net packages
- [x] blazor server: Quilt4Net.Toolkit.Health 0.6.9 → 0.6.16
- [x] blazor-platform server: Quilt4Net.Toolkit.Health/Api/Blazor 0.6.9 → 0.6.16

### 4. Update Microsoft + Radzen packages
- [x] blazor Client: Microsoft.AspNetCore.Components.WebAssembly 10.0.5 → 10.0.6, Radzen.Blazor 10.2.0 → 10.2.3
- [x] blazor-platform Client: Microsoft.AspNetCore.Components.WebAssembly/Authorization 10.0.5 → 10.0.6, Radzen.Blazor 10.2.0 → 10.2.3
- [x] blazor + blazor-platform server: Microsoft.AspNetCore.Components.WebAssembly.Server 10.0.5 → 10.0.6

### 5. Build verification
- [x] `dotnet build -c Release` passes, 0 warnings
- [x] Fast test suite passes (14/14, ~2 min)
- [~] Representative build test per template (running)

### 6. Commit
- [ ] Single commit with all 13 package bumps

## Last session
Updates applied, build passes, fast tests green. Running 3 build tests (one per template) to verify generated projects compile with new packages.
