# Plan: NuGet packaging and CI

## Steps

### 1. Review and improve Tharga.Starter.csproj packaging
- [ ] Verify all 3 templates are included via Content glob
- [ ] Add missing NuGet metadata: PackageLicenseExpression, RepositoryUrl, PackageProjectUrl, PackageIcon (if available)
- [ ] Remove hardcoded PackageVersion (should come from CI)
- [ ] Exclude test project files from pack output

### 2. Local pack verification
- [ ] Run `dotnet pack -c Release` and confirm .nupkg is produced
- [ ] Inspect the .nupkg contents to confirm all 3 templates are present with correct structure
- [ ] Test install from local .nupkg: `dotnet new install ./bin/Release/Tharga.Starter.*.nupkg`
- [ ] Create a project from each template (console, blazor, blazor-platform) and verify builds
- [ ] Uninstall the package afterwards

### 3. Add test for pack + install workflow
- [ ] Add integration test: `PackAndInstallTests.cs` that runs `dotnet pack` and verifies the .nupkg structure
- [ ] Skip on CI environments where it would be redundant

### 4. Create GitHub Actions workflow
- [ ] Create `.github/workflows/build.yml` following the Tharga.Platform pattern
- [ ] Jobs: build+test, security (CodeQL), release (master push), prerelease (PR)
- [ ] MAJOR_MINOR env var: `0.1` (matches current PackageVersion)
- [ ] Ubuntu runner, .NET 9/10 SDKs
- [ ] Pack step: only the single Tharga.Starter.csproj

### 5. Update README
- [ ] Add install instructions from nuget.org (`dotnet new install Tharga.Starter`)
- [ ] Keep local-path install instructions as alternative for development

### 6. Run full test suite and verify
- [ ] Ensure all existing 47 tests still pass
- [ ] Commit changes

## Last session
Plan created. Awaiting user approval before implementation.
