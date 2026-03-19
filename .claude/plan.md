# Plan: Blazor UI Foundation

## Steps

- [x] 1. Update server .csproj — add `Tharga.Blazor` 2.0.0-pre.4 package, remove Nullable
- [x] 2. Update client .csproj — add `Radzen.Blazor` package, remove Nullable
- [x] 3. Update server Program.cs — add Radzen + ThargaBlazor registration
- [x] 4. Update client Program.cs — add Radzen registration
- [x] 5. Update _Imports.razor (both projects) — add Radzen/Tharga usings
- [x] 6. Update App.razor — add RadzenTheme, Tharga.Blazor.js, Radzen.Blazor.js
- [x] 7. Replace MainLayout.razor — use RadzenLayout with CustomErrorBoundary, Title, BreadCrumbs
- [x] 8. Replace NavMenu.razor — use RadzenHeader, RadzenSidebar, RadzenPanelMenu with conditional samples
- [x] 9. Add Constants.cs — theme storage name
- [x] 10. Update Home.razor — add StandardButton demo
- [x] 11. Add About.razor — version, environment, theme selector
- [x] 12. Make Counter and Weather pages conditional with `IncludeSamples` parameter
- [x] 13. Update template.json — add `IncludeSamples` symbol, conditional file exclusion
- [x] 14. Remove Nullable from test project .csproj files (already removed)
- [x] 15. Update integration tests — fixed assertion for prerender:false mode
- [~] 16. Build and test — template builds OK, template tests pass, starter tests running
