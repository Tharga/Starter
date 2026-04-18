# Feature: Enhanced Developer Pages

## Goal
Enhance the blazor-platform template's developer pages to match Quilt4Net.Server's feature set.

## Originating branch
`develop`

## Scope
1. Add new optional flag `--IncludeCache` that wires up Tharga.Cache.Blazor + a Cache developer page
2. Enhance Database page with MonitorToolbar + reset handlers (no simulator)
3. Enhance Log page with deep-linking (Tab parameter, DetailPath, SummaryPath)
4. Add new LogDetail page at `/developer/log/detail/{Id}`
5. Add new LogSummary page at `/developer/log/summary/{Fingerprint}`

## Out of scope
- Database Simulator tab (per user, skipped)
- Content, Configuration, Users, DeveloperAudit pages (already aligned)
- Supporting anything older than Tharga.Cache 0.4.1

## Acceptance criteria
- [ ] `--IncludeCache` flag added to template.json (default: false)
- [ ] Cache page exists when IncludeCache=true, is excluded when false
- [ ] Tharga.Cache.Blazor 0.4.1 package referenced + `AddCache()` registered, both conditional
- [ ] NavMenu shows Cache link under Developer when IncludeCache=true
- [ ] Database page shows MonitorToolbar above tabs with working reset callbacks
- [ ] Log page supports Tab query parameter, links to LogDetail/LogSummary
- [ ] LogDetail and LogSummary pages exist with breadcrumb handling
- [ ] All existing tests pass
- [ ] New tests cover: IncludeCache on/off, MonitorToolbar presence, Log deep-linking
- [ ] Template builds for all flag combinations

## Done condition
All acceptance criteria met, tests pass, user confirms.
