# Feature: Update all NuGet packages

## Goal
Bring every hardcoded NuGet package reference across the repo up to its latest stable version.

## Originating branch
`develop`

## Scope
All `.csproj` files in:
- `Tharga.Starter.csproj` (project root — metadata only, no packages)
- `tests/Tharga.Starter.Tests/` (test harness)
- `templates/console/**/` (all templates include test projects)
- `templates/blazor/**/`
- `templates/blazor-platform/**/`

All package types: Tharga, Quilt4Net, Microsoft, Radzen, test SDKs.

## Out of scope
- Changing which packages are used (no additions/removals)
- Wildcard versions (`2.*`, `3.*`) — leave as-is since they resolve to latest automatically
- Major version bumps that introduce breaking changes (evaluate case-by-case)

## Acceptance criteria
- [ ] All hardcoded package versions reflect latest stable as of 2026-04-18
- [ ] Changelogs reviewed for any breaking changes in the larger jumps (Tharga.Team 2.0.5→2.0.10, Quilt4Net 0.6.9→0.6.16)
- [ ] `dotnet build -c Release` passes
- [ ] All fast tests pass
- [ ] Representative slow tests (one build per template) pass
- [ ] No new build warnings introduced

## Done condition
All acceptance criteria met, tests pass, user confirms.
