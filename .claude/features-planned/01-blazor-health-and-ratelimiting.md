# Feature: Health Endpoints & Rate Limiting options for tharga-blazor

## Goal
Add `--IncludeHealth` and `--IncludeRateLimiting` optional flags to the existing `tharga-blazor` template.

## Scope
- Add `Quilt4Net.Toolkit.Health` with `AddQuilt4NetHealth()` / `UseQuilt4NetHealth()` behind `--IncludeHealth` flag (default false)
- Add built-in rate limiting with `AddRateLimiter()` / `UseRateLimiter()` behind `--IncludeRateLimiting` flag (default false)
- Update template.json with new symbols
- Add conditional code in Program.cs and .csproj
- Add integration tests for all flag combinations

## Acceptance criteria
- Template builds with all combinations of flags
- Health endpoints work when enabled
- Rate limiting works when enabled
- Integration tests cover all variants
