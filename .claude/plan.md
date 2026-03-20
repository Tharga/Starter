# Plan: Health Endpoints & Rate Limiting options

## Steps

- [~] 1. Add `IncludeHealth` and `IncludeRateLimiting` symbols to template.json
- [ ] 2. Add conditional `Quilt4Net.Toolkit.Health` package reference to server .csproj
- [ ] 3. Add conditional health registration in Program.cs (`AddQuilt4NetHealth()` / `UseQuilt4NetHealth()`)
- [ ] 4. Add conditional rate limiting registration in Program.cs (`AddRateLimiter()` / `UseRateLimiter()`)
- [ ] 5. Add integration tests for all 4 flag combinations (none, health, rate limiting, both)
- [ ] 6. Update README feature table to mark Health and Rate Limiting for `tharga-blazor`
- [ ] 7. Build and test all variants
