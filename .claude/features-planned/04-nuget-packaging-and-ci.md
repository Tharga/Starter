# Feature: NuGet packaging and CI

## Goal
Set up the NuGet template pack build so that `Tharga.Starter` can be published and installed via `dotnet new install Tharga.Starter`.

## Scope
- Verify `Tharga.Starter.csproj` includes all three blazor templates + console template
- Add GitHub Actions workflow for build, test, and NuGet pack
- Add versioning strategy
- Test install from local .nupkg

## Acceptance criteria
- `dotnet pack` produces a valid .nupkg with all templates
- All templates are installable from the .nupkg
- Generated projects build and pass tests
