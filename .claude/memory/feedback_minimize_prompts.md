---
name: Minimize permission prompts
description: Settings should use broad wildcard patterns to avoid repeated permission prompts for routine operations
type: feedback
---

Use broad wildcard patterns in settings.json to avoid repeated permission prompts.

**Why:** The user was prompted repeatedly for routine operations like `cd /c/dev/tharga/...`, `git diff`, `git log`, `git show` when exploring other Tharga repos for API changes. This interrupts flow.

**How to apply:** When adding Bash allows, use `Bash(command *)` wildcards. For Read, use `$DOC_ROOT/**` and `$DEV_ROOT/tharga/**` to cover all Tharga projects. Keep settings.json generic (wildcards); settings.local.json for machine-specific paths only.
