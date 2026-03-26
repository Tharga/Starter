# Plan: Blazor Platform Template

## Steps

- [x] 1. Create feature branch `feature/blazor-platform-template`
- [x] 2. Copy `templates/blazor/` to `templates/blazor-platform/` as starting point
- [x] 3. Add packages: `Tharga.Team.Blazor`, `Tharga.Team.Service`, `Tharga.Team.MongoDB`, `Tharga.MongoDB.Blazor`
- [x] 4. Create entity/service types (UserEntity, TeamEntity, TeamMember, AppTeamService, AppUserService, TeamCookieClaimsTransformation)
- [x] 5. Wire up Program.cs (server) with all steps (Auth, Controllers, Teams, API Keys, Scopes, Roles, Audit, MongoDB)
- [x] 6. Update `_Imports.razor` (both projects) with required usings
- [x] 7. Add appsettings.json sections (AzureAd, ConnectionStrings) with placeholders
- [x] 8. Add pages: Profile, Team, Users, ApiKeys, AuditLog
- [x] 9. Update NavMenu with LoginDisplay, TeamSelector, and auth-gated admin links
- [x] 10. Update template.json with identity `Tharga.Blazor.Platform` and short name `tharga-blazor-platform`
- [x] 11. Add 14 integration tests for the new template — all pass
- [x] 12. Tharga.Starter.csproj already includes `templates/**/*` — no changes needed
- [x] 13. Update README with new template
- [ ] 14. Commit all changes
