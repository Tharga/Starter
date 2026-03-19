# Feature: Blazor Web App Template

## Goal
Create a `dotnet new` template for a Blazor Web App with interactive render mode Auto (Server and WebAssembly), including a test project.

## Originating branch
develop

## Scope
- Blazor Web App solution template under `templates/blazor/` with:
  - Server project (Tharga.Blazor1)
  - Client project (Tharga.Blazor1.Client)
  - Test project (Tharga.Blazor1.Tests)
  - Solution file (Tharga.Blazor1.slnx)
  - `.template.config/template.json` — template configuration
- Short name: `tharga-blazor`
- Name substitution: `Tharga.Blazor1` → user-provided name
- Interactive render mode: Auto (Server + WebAssembly)

## Acceptance criteria
- [ ] `dotnet new install ./templates/blazor` installs the template
- [ ] `dotnet new tharga-blazor -n TestApp` generates a buildable solution
- [ ] Generated solution contains server, client, and test projects
- [ ] `dotnet build` and `dotnet test` pass on the generated solution
- [ ] Name substitution works correctly (project names, namespaces, sln references)
- [ ] Interactive render mode Auto is configured correctly
- [ ] Template appears in Visual Studio's "Create a new project" dialog

## Done condition
All acceptance criteria met, user confirms feature is complete.
