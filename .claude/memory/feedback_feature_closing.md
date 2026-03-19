---
name: Feature closing workflow
description: When closing a feature, merge the feature branch into develop with a merge commit (not squash, not fast-forward)
type: feedback
---

When closing a feature, merge the feature branch into develop using `git merge --no-ff` to create a merge commit. Do not squash, do not fast-forward, do not cherry-pick.

**Why:** The user wants the full feature branch history preserved, with a merge commit on develop marking the feature completion.

**How to apply:** After feature is confirmed done, checkout develop, run `git merge --no-ff feature/<name> -m "feat: <name> complete"`, then delete the local feature branch. Do not push or manage origin — user handles that.
