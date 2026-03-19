## Session Continuity

### Starting a session
1. Run `git status` to check for uncommitted changes
   - If uncommitted changes exist, alert me immediately and stop
   - Do not proceed until I have confirmed how to handle them (commit, stash, or discard)
2. Check if `.claude/mission.md` exists and read the project mission and context.
3. Check if `.claude/plan.md` exists.
   If it does, read it and summarize what has been done and what the next step is.
   If it does not exist, ask me how I would like to proceed.
4. Check if `.claude/feature.md` exists and read the current feature scope.

### During a session
After completing each step in the plan:
- Mark it as `[x]` done in `.claude/plan.md`
- Add a brief note about what was done and any important decisions made
- Mark the next step as `[~]` in progress

### Ending a session
- Update `.claude/plan.md` with the current status of all steps
- Add a "Last session" note summarizing what was completed and what comes next

## Build & Test
```bash
dotnet build -c Release
dotnet test -c Release
```

## Template Guidelines
- Each template must have a valid `.template.config/template.json`
- Use `tharga-` prefix for all template shortNames (e.g. `tharga-blazor`, `tharga-console`)
- Test templates by installing locally: `dotnet new install ./templates/<name>`
- After changes, verify with: `dotnet new <shortName> -n TestProject && dotnet build TestProject`

## Coding Guidelines
- Keep an empty line between constructor and functions
- Use `init` over `set` wherever possible
- Prefer functional programming patterns

### Feature and framework organization
- Place feature-specific code under `features/[name]`
- Place shared cross-functional code under `framework/[name]`

## Workflow Rules
- Before making changes, explain what you plan to do
- After completing a task, summarize what was changed
- If unsure about something, ask before proceeding

## Git Rules
- Never push to remote without explicit approval from me
- Never force push under any circumstances
- Create branch `feature/<feature-name>` at the start of each feature
- Commit at logical milestones
- Use conventional commits: `feat:`, `fix:`, `test:`, `docs:`
- Never merge to main — leave that for me to review and merge

## Feature Workflow

### Planning features
- Multiple features can be planned ahead in `.claude/features-planned/`
- Each file represents one feature and they are executed in order (e.g. `01-feature-name.md`, `02-feature-name.md`)
- When starting a new feature, check `features-planned/` first for the next planned feature

### Starting a feature
When told to start a new feature:
1. Ask for the feature name and goal if not provided
2. Note the current branch as the originating branch for the feature
3. Create a new branch: `git checkout -b feature/<feature-name>`
4. Create `.claude/feature.md` with goal, scope, acceptance criteria, and done condition
5. Create or update `.claude/plan.md` with the steps to implement the feature
6. Confirm the plan before starting any code changes

### Completing implementation
When all planned steps are done:
- All tests pass
- Commit all changes
- Summarize what was done and ask the user to test and provide feedback
- Do NOT close the feature — wait for the user to confirm it is done

### Closing a feature (only when the user says it is done)
- All acceptance criteria in `.claude/feature.md` are met
- All tests pass
- `.claude/feature.md` is archived to `.claude/features-done/<feature-name>.md` and both `.claude/feature.md` and `.claude/plan.md` should be deleted
- Remove the corresponding file from `.claude/features-planned/` if one exists
- A final commit is made with message: `feat: <feature-name> complete`
- Merge to originating branch and delete feature branch only when the user explicitly asks
