# Opening a pull request

**This repo has two remotes — check `git remote -v` before opening a PR, don't assume `origin` is the target:**

- `karaoke` → `karaoke-dev/osu-framework-font` — **this is the canonical upstream repo. PRs go here.**
- `origin` → `andy840119/osu-framework-font` — a personal fork/mirror. A PR opened against this one is easy to merge into the wrong place by mistake (it happened once — see PRs #183/#184, which were merged into the fork and then had to be re-opened against `karaoke-dev/osu-framework-font` as new PRs).

Confirm which is which with `gh api repos/karaoke-dev/osu-framework-font --jq .permissions` (push access implies it's a legitimate target) rather than guessing from remote name alone — org names have changed before (`osu-karaoke` → `karaoke-dev` in the sibling `osu-framework-microphone` repo, which redirects automatically).

## Workflow

```
git fetch karaoke master
git checkout -b <branch> karaoke/master
# ...make changes, commit...
git push -u karaoke <branch>
gh pr create --repo karaoke-dev/osu-framework-font --base master --head <branch>
```

Do **not** default to `origin`/`andy840119/osu-framework-font` unless the user explicitly asks for that fork specifically.

## If CI doesn't run on the PR

Check `gh api repos/<owner>/<repo>/actions/permissions` — Actions can simply be disabled on a personal fork (`"enabled": false`), which is a repo-settings problem, not a workflow-file problem. Confirm the workflow YAML itself is correct before concluding CI is broken.
