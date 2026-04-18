# Feature: NuGet packaging and CI

## Goal
Set up NuGet template pack build and GitHub Actions CI so `Tharga.Starter` can be published to nuget.org and installed via `dotnet new install Tharga.Starter`.

## Originating branch
`develop`

## Scope
1. Verify/improve `Tharga.Starter.csproj` packaging (all 3 templates included, metadata correct)
2. Add `dotnet pack` verification — produce valid .nupkg locally
3. Add local install test — verify templates work when installed from the .nupkg
4. Add GitHub Actions workflow mirroring the Tharga.Platform pattern:
   - Build + test on push/PR
   - CodeQL security scan
   - Release on push to master → publish to NuGet + create GitHub release
   - Prerelease on PR → publish prerelease to NuGet
5. Versioning strategy: `MAJOR.MINOR.PATCH` auto-computed from git tags (same as Platform)

## Out of scope
- Actually publishing the first release to nuget.org (user's decision)
- Changes to template content itself

## Acceptance criteria
- [ ] `dotnet pack -c Release` produces a valid `.nupkg` locally
- [ ] The `.nupkg` contains all three templates (console, blazor, blazor-platform)
- [ ] `dotnet new install <path-to-nupkg>` installs successfully
- [ ] After install, all three `dotnet new tharga-*` shortnames work and produce buildable projects
- [ ] GitHub Actions workflow file is valid and runs build+test on PR
- [ ] Workflow produces the .nupkg as an artifact
- [ ] Release job is gated on push to master
- [ ] README updated with install instructions from nuget.org

## Done condition
All acceptance criteria met, tests pass, user confirms.
