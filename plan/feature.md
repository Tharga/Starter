# Feature: Update blazor-platform template to current package versions

Source: https://github.com/Tharga/Starter/issues/3 (also covers #2, see Scope).

## Goal
New projects created from the `blazor-platform` template start on current package majors
(Tharga.Team 4.x, Quilt4Net.Toolkit 1.x, Tharga.MongoDB.Blazor 2.21.x) instead of having to
migrate before writing any feature code. A new Tharga.Team site is waiting on this.

## Originating branch
`develop`

## Scope
1. Update every package in `blazor-platform` and the other templates to the latest stable versions, majors included.
   Read release notes / upgrade guides for each major first (Tharga.Team 2.x to 4.x, Quilt4Net.Toolkit 0.x to 1.x).
2. Bring template code up to what the new packages expect (current Tharga.Team entry point instead of
   hand-wired services, new required registrations).
3. Fold in #2: add `AddQuilt4NetContent()` to the `IncludeQuilt4Net` block in `Program.cs` (verify it is still the right call on Toolkit 1.x).
4. Confirm no NU190x vulnerability warnings on a clean restore, or list the remaining ones and why.
5. Verify `dotnet new` still produces a working project with and without each option
   (`IncludeQuilt4Net`, `IncludeHealth`, `IncludeCache`).
6. Update the template README to describe what the template contains now.

## Out of scope
- Publishing a release to nuget.org (user's decision).
- The `/developer/version-matrix` page request (separate, follows after this).
- New template features beyond what the upgrades require.

## Acceptance criteria
- [ ] All package references in all templates are at latest stable (floating versions left as they are).
- [ ] Template code compiles and runs against the new majors; no hand-wired registrations that the new packages replace.
- [ ] #2 fixed: `CanConnectAsync(Service.Content)` does not throw with only `Quilt4Net:ApiKey` configured.
- [ ] Full Starter test suite passes; test count read and compared with before (baseline recorded in plan.md).
- [ ] Template integration tests pass.
- [ ] Clean restore shows no NU190x warnings, or the remainder is documented.
- [ ] `dotnet new` output builds for each option combination.
- [ ] Template README updated.

## Done condition
All acceptance criteria met, tests pass, user confirms. Closing then also closes #3 and #2 on GitHub (with confirmation).
