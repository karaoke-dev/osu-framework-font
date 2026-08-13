# Tag releases

Pushing a tag to the canonical repo triggers [`.github/workflows/deploy-pack.yml`](../.github/workflows/deploy-pack.yml) (`Tagged Release`), which builds `osu.Framework.Font`, packs it with `/p:Version=<tag>`, and publishes the `.nupkg` to nuget.org.

## Tag naming convention

`YYYY.MMDD.PATCH`, matching this repo's own version history (`git tag -l | sort -V`), e.g.:

```
2025.0511.0
2025.0607.0
2026.0718.0
2026.0813.0
```

- `YYYY` — 4-digit year.
- `MMDD` — 4-digit, zero-padded month + day (e.g. August 13th → `0813`, not `813`).
- `PATCH` — starts at `0` for the first release of that day; bump to `1`, `2`, ... if a second release goes out the same day (e.g. `2023.0627.0` then `2023.0627.1`).
- Pre-releases use a `-alpha` suffix on the patch segment, e.g. `2022.0806.1-alpha`.

There's no separate `v` prefix — tags are the bare version string, since that string is passed straight through as the package's `Version`.

## Which remote to push the tag to

Push tags to `karaoke` (`karaoke-dev/osu-framework-font`), **not** `origin` (the personal fork). Two independent reasons this matters, not just convention:

1. The `Tagged Release` workflow only runs where it's pushed. A tag pushed to `origin` runs whatever workflow exists on that fork (if Actions are even enabled there — see [opening-a-pull-request.md](opening-a-pull-request.md)), not the canonical one.
2. Publishing now uses [nuget.org Trusted Publishing](https://learn.microsoft.com/en-us/nuget/nuget-org/trusted-publishing) (OIDC, via `NuGet/login@v1` in the `release` job) instead of a long-lived API key secret. The trusted publishing policy on nuget.org is locked to a specific `(repository owner, repository, workflow file)` triple — currently `karaoke-dev` / `osu-framework-font` / `deploy-pack.yml`. A run from any other repo (including the personal fork) will fail to exchange its OIDC token for a NuGet API key, even if the workflow file itself is identical.

```
git fetch karaoke master
git tag 2026.0813.0        # on the commit you want released, following the convention above
git push karaoke 2026.0813.0
```

Then watch it with `gh run list --repo karaoke-dev/osu-framework-font --workflow=deploy-pack.yml --limit 1`.
