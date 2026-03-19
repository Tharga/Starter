# Feature: Blazor UI Foundation (Tharga.Blazor)

## Goal
Update the `tharga-blazor` template to include Tharga.Blazor UI Foundation (Step 1 from Tharga Platform), replacing the default Bootstrap layout with Radzen-based layout and components.

## Originating branch
develop

## Scope
- Replace Bootstrap layout with Radzen layout (RadzenLayout, RadzenHeader, RadzenSidebar, RadzenBody, RadzenFooter)
- Add `Tharga.Blazor` package with `AddThargaBlazor()`, `CustomErrorBoundary`, `Title`, `BreadCrumbs`
- Add Radzen theme + cookie theme service
- Add `Constants.cs` with theme storage name
- Update Home page (simple hello world with StandardButton)
- Add About page (version, environment, theme selector)
- Keep Error + NotFound pages
- Add `--IncludeSamples` template option (default true) for Counter and Weather pages
- Remove `<Nullable>enable</Nullable>` from all projects
- Keep integration test project

## Acceptance criteria
- [ ] Template installs and generates a project that builds
- [ ] Generated project uses Radzen layout with Tharga.Blazor components
- [ ] `--IncludeSamples false` produces a minimal project without Counter/Weather
- [ ] `--IncludeSamples true` (default) includes Counter and Weather sample pages
- [ ] No `<Nullable>enable</Nullable>` in any project file
- [ ] Integration tests pass
- [ ] Starter solution tests pass

## Done condition
User confirms the template works correctly
