# Feature: tharga-blazor-platform template

## Goal
Create a new `tharga-blazor-platform` template that includes features 1–9: base UI, authentication, API logging, remote config, content, MongoDB, team management, and API controllers.

## Scope
- New template under `templates/blazor-platform/` based on `templates/blazor/`
- Feature 3: Authentication via `Tharga.Team.Blazor` (`AddThargaAuth()` / `UseThargaAuth()`)
- Feature 4: API Logging via `Quilt4Net.Toolkit.Api` (`AddQuilt4NetApiLogging()` / `UseQuilt4NetApiLogging()`)
- Feature 5: Remote Config via `Quilt4Net.Toolkit.Blazor` (`AddQuilt4NetRemoteConfiguration()`)
- Feature 6: Content & Localization via `Quilt4Net.Toolkit.Blazor` (`AddQuilt4NetBlazorContent()`)
- Feature 7: MongoDB via `Tharga.MongoDB.Blazor` (`AddMongoDB()` / `UseMongoDB()`)
- Feature 8: Team Management via `Tharga.Team.MongoDB`
- Feature 9: API Controllers via `AddThargaControllers()` / `UseThargaControllers()`
- Optional flags: `--IncludeHealth`, `--IncludeRateLimiting`, `--IncludeQuilt4Net` (for features 4-6)
- Placeholder config values in appsettings.json with clear comments
- Developer pages (Log, Configuration, Content, Database, User)
- Update Tharga.Starter.csproj to include new template
- Integration tests for the new template

## Depends on
- Feature 01 (Health & Rate Limiting patterns established)

## Acceptance criteria
- Template installs and generates a project that builds
- Auth flow is wired up (with placeholder config)
- MongoDB, Team, API controllers are configured
- Optional flags work correctly
- All integration tests pass
- README feature table updated with new template column
