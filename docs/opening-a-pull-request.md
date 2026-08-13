# Opening a pull request

**This repo has two remotes — check `git remote -v` before opening a PR:**

- `karaoke` → `karaoke-dev/osu-framework-font` — the canonical upstream repo. **PRs target this repo as their base**, but branches aren't pushed here directly.
- `origin` → `andy840119/osu-framework-font` — personal fork. **Push branches here**, then open the PR cross-repo against `karaoke-dev/osu-framework-font`.

Confirm which is which with `gh api repos/karaoke-dev/osu-framework-font --jq .permissions` (push access implies it's a legitimate target) rather than guessing from remote name alone — org names have changed before (`osu-karaoke` → `karaoke-dev` in the sibling `osu-framework-microphone` repo, which redirects automatically).

Don't confuse "push to `origin`" with "PR against `origin`" — those are different repos. A PR whose **base repo** is `andy840119/osu-framework-font` is wrong and easy to create by accident (it happened once — see PRs #183/#184, which were merged into the fork and then had to be re-opened against `karaoke-dev/osu-framework-font` as new PRs). The base repo must always be `karaoke-dev/osu-framework-font`; only the head branch lives on `origin`.

## Workflow

```
git fetch karaoke master
git checkout -b <branch> karaoke/master
# ...make changes, commit...
git push -u origin <branch>
gh pr create --repo karaoke-dev/osu-framework-font --base master --head andy840119:<branch>
```

The `--head andy840119:<branch>` form is required for a cross-repo PR — a bare `--head <branch>` would look for the branch inside `karaoke-dev/osu-framework-font` itself and fail (or worse, silently target the wrong branch if one with the same name happens to exist there).

## If CI doesn't run on the PR

`dotnet-core.yml` triggers on `pull_request: branches: [master]`, so once the PR's base repo is correctly `karaoke-dev/osu-framework-font`, CI runs there regardless of the fork's own Actions settings. If it still doesn't run, check `gh api repos/karaoke-dev/osu-framework-font/actions/permissions` and confirm the PR's base repo is actually `karaoke-dev/osu-framework-font` and not the fork — Actions being disabled on the personal fork only matters if the PR was mistakenly opened against the fork itself.
