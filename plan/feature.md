# Feature: Template Fixes (Blazor Platform)

## Goal
Address 5 pending requests from Florida that fix bugs and align the `tharga-blazor-platform` template with Platform.Sample patterns.

## Originating branch
`develop`

## Scope
1. Fix ContentAdmin wrong namespace in Content.razor
2. Register audit scopes in AddThargaScopes so Audit menu works out of the box
3. Fix claim extraction in AppUserService (use GetEmail()/GetDisplayName() from Tharga.Toolkit)
4. Set TeamMember.Name in AppTeamService.CreateTeam using displayName parameter
5. Add LanguageSelector to NavMenu (conditional on IncludeQuilt4Net)

## Out of scope
- Changes to the base `tharga-blazor` template (no auth/team there)
- Changes to the console template
- New template features or flags

## Acceptance criteria
- [ ] Content.razor uses correct namespace `Quilt4Net.Toolkit.Blazor.Features.Language.ContentAdmin`
- [ ] Audit scopes (`audit:read`, `audit:apikey`) are registered in AddThargaScopes at appropriate access levels
- [ ] AppUserService uses `GetEmail()` and `GetDisplayName()` from Tharga.Toolkit
- [ ] AppTeamService.CreateTeam sets `Name = displayName` on the owner TeamMember
- [ ] LanguageSelector is included in NavMenu when IncludeQuilt4Net is enabled
- [ ] All existing tests pass
- [ ] New tests verify the fixes

## Done condition
All acceptance criteria met, all tests pass, user confirms.
