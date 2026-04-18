# Plan: Enhanced Developer Pages

## Steps

### 1. Add IncludeCache flag to template.json
- [ ] Add `IncludeCache` parameter (bool, default false)
- [ ] Add modifier to exclude Cache.razor when IncludeCache=false

### 2. Conditional Tharga.Cache.Blazor package
- [ ] Add `<!--#if (IncludeCache) -->` block with `Tharga.Cache.Blazor` v0.4.1 in csproj

### 3. Conditional Cache registration in Program.cs
- [ ] Add `#if (IncludeCache)` block with `builder.Services.AddCache();`

### 4. Create Cache.razor developer page
- [ ] `/developer/cache` with RadzenStack containing SummaryView + ListView in RadzenCards
- [ ] Breadcrumb handling
- [ ] Authorize by Roles.Developer

### 5. Update _Imports.razor
- [ ] Add `@using Tharga.Cache.Blazor` conditional on IncludeCache

### 6. Update NavMenu.razor
- [ ] Add Cache menu item under Developer, conditional on IncludeCache

### 7. Update Database.razor
- [ ] Add MonitorToolbar inside a RadzenCard above RadzenTabs
- [ ] Add @ref to CollectionView and CallView
- [ ] Add OnCallsReset and OnCacheReset handlers

### 8. Update Log.razor
- [ ] Add Tab query parameter
- [ ] Pass Context=ApplicationInsightsContextExtensions.Current, DetailPath, SummaryPath, Tab to LogView
- [ ] Add breadcrumb unlink for "log"

### 9. Create LogDetail.razor
- [ ] `/developer/log/detail/{Id}` page
- [ ] Decode navigation params, add virtual breadcrumb segments
- [ ] Render LogDetailView

### 10. Create LogSummary.razor
- [ ] `/developer/log/summary/{Fingerprint}` page
- [ ] Breadcrumb relinking
- [ ] Render LogSummaryView

### 11. Update tests
- [ ] Test: IncludeCache=true includes Cache.razor, Cache nav item, and Tharga.Cache.Blazor package reference
- [ ] Test: IncludeCache=false excludes all Cache artifacts
- [ ] Test: Database.razor contains MonitorToolbar
- [ ] Test: Log.razor contains DetailPath/SummaryPath/Tab
- [ ] Test: LogDetail.razor and LogSummary.razor exist
- [ ] Run fast tests + representative slow test

### 12. Commit
