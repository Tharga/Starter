# Feature: Health Endpoints & Rate Limiting options for tharga-blazor

## Goal
Add `--IncludeHealth` and `--IncludeRateLimiting` optional flags to the existing `tharga-blazor` template.

## Originating branch
master

## Scope
- Add `Quilt4Net.Toolkit.Health` with `AddQuilt4NetHealth()` / `UseQuilt4NetHealth()` behind `--IncludeHealth` flag (default false)
- Add built-in rate limiting with `AddRateLimiter()` / `UseRateLimiter()` behind `--IncludeRateLimiting` flag (default false)
- Update template.json with new symbols
- Add conditional code in Program.cs and .csproj
- Add integration tests for all flag combinations

## Acceptance criteria
- [ ] Template builds with all combinations of flags (none, health only, rate limiting only, both)
- [ ] Health endpoints respond when enabled
- [ ] Rate limiting is configured when enabled
- [ ] No `<Nullable>enable</Nullable>` in any project file
- [ ] Integration tests cover all variants
- [ ] README feature table updated

## Done condition
User confirms the template works correctly
