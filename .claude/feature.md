# Feature: Console Template

## Goal
Create a minimal `dotnet new` console template with a test project, and set up the template pack infrastructure so templates can be built and installed.

## Originating branch
master

## Scope
- Template pack project (`Tharga.Starter.csproj`) at repo root
- Console solution template under `templates/console/` with:
  - `Tharga.Console1/` — the console app project
  - `Tharga.Console1.Tests/` — integration-style test project
  - `Tharga.Console1.sln` — solution file
  - `.template.config/template.json` — template configuration
- Short name: `tharga-console`
- Name substitution: `Tharga.Console1` → user-provided name

## Acceptance criteria
- [ ] `dotnet pack Tharga.Starter.csproj` produces a NuGet package
- [ ] `dotnet new install ./templates/console` installs the template
- [ ] `dotnet new tharga-console -n TestApp` generates a buildable solution
- [ ] Generated solution contains both app and test projects
- [ ] `dotnet build` and `dotnet test` pass on the generated solution
- [ ] Name substitution works correctly (project names, namespaces, sln references)

## Done condition
All acceptance criteria met, user confirms feature is complete.
