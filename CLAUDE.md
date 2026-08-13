# osu.Framework.KaraokeFont

Unofficial osu!framework extension for extra font/text effects (outline, shadow, step shaders). Single desktop target (net8.0): `osu.Framework.Font` (library) + `osu.Framework.Font.Tests` (NUnit + visual test browser).

## Playbooks

Detailed, recurring-task playbooks live under [`docs/`](docs/) so this file stays short:

- [docs/upgrading-dependencies.md](docs/upgrading-dependencies.md) — the "update the important packages, fix any test failures, one commit per category" task: dependabot commit grouping, cross-checking versions against `ppy/osu-framework`, the NUnit 3 → 4 `Assert.*` migration, and the local verification checklist.
- [docs/opening-a-pull-request.md](docs/opening-a-pull-request.md) — this repo has two remotes (`karaoke` = canonical upstream, `origin` = personal fork); PRs go to `karaoke`, not `origin`.
- [docs/tag-releases.md](docs/tag-releases.md) — tag naming convention (`YYYY.MMDD.PATCH`) and why release tags must be pushed to `karaoke` (nuget.org Trusted Publishing is locked to that repo).
