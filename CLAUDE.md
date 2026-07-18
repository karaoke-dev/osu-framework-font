# osu.Framework.KaraokeFont

Unofficial osu!framework extension for extra font/text effects (outline, shadow, step shaders). Single desktop target (net8.0): `osu.Framework.Font` (library) + `osu.Framework.Font.Tests` (NUnit + visual test browser).

## Updating dependencies

This is a recurring task ("update the important packages, fix any test failures, one commit per category"). Follow this playbook — it encodes real breakage found while doing this by hand.

### 1. Commit grouping from `.github/dependabot.yml`

- **osu-related** group: `ppy.osu.Framework`, `ppy.osu.Game.Resources` — bump together, one commit.
- Everything else (`Microsoft.NET.Test.Sdk`, `NUnit`, `NUnit3TestAdapter`) is ungrouped — one commit each, matching how dependabot PRs land individually for these.

Check with `dotnet list <csproj> package --outdated` per project (`dotnet list package --outdated` doesn't work directly on a solution/slnf-less repo layout here, but this repo has no `.slnf` — just build/restore at the repo root or per-csproj).

### 2. Cross-check versions against `ppy/osu-framework` before trusting "latest"

This repo depends directly on `ppy.osu.Framework`, so a "latest" bump can be a large jump (e.g. a full year of upstream releases). Before assuming a compile error is *your* bug:

- Search upstream for the removed/changed symbol: `gh api "search/code?q=repo:ppy/osu-framework+filename:<Name>.cs"` to find where a type now lives, then `gh api "repos/ppy/osu-framework/commits?path=<file>"` to read *why* it changed (the commit message usually explains whether there's a real replacement or the old member was already dead).
- Example hit from 2025.604.1 → 2026.629.0: `IShader.GetUniform<T>()` was removed (`osu.Framework/Graphics/Shaders/IShader.cs`). The upstream removal commit explained the method had been non-functional since `GlobalPropertyManager` was deleted years earlier — so the fix was deleting the matching dead passthrough in `ICustomizedShader`/`CustomizedShader`/`StepShader` (which already just threw `NotSupportedException` in `StepShader`, confirming nothing real depended on it), not finding a replacement API.
- For `NUnit`/`NUnit3TestAdapter`/`Microsoft.NET.Test.Sdk`, this repo's own dependabot history tracks NuGet's latest independently of `ppy/osu-framework` (which pins much older test-tooling versions upstream) — keep following latest for these three.

### 3. NUnit 3 → 4 major bump breaks classic `Assert.*` calls

NUnit 4 removed the classic assertion methods (`Assert.AreEqual`, `AreNotEqual`, `Throws<T>(...)`, `Catch<T>(...)`) from the `Assert` class — they now live under `NUnit.Framework.Legacy.ClassicAssert` (separate namespace/DLL). Two migration paths; **use the constraint model** (what upstream NUnit recommends, and what was done here), not the `ClassicAssert` shim:

```csharp
Assert.AreEqual(expected, actual);        // ->  Assert.That(actual, Is.EqualTo(expected));
Assert.AreNotEqual(expected, actual);     // ->  Assert.That(actual, Is.Not.EqualTo(expected));
Assert.Throws<TEx>(() => Code());         // ->  Assert.That((Action)(() => Code()), Throws.TypeOf<TEx>());
Assert.Catch<TEx>(() => Code());          // ->  Assert.That((Action)(() => Code()), Throws.InstanceOf<TEx>());
```

Gotcha: `Assert.That(() => ..., constraint)` is **ambiguous** (`CS0121`) between the `TestDelegate` and `Action` overloads when the argument is a bare lambda — NUnit kept both overloads and their signatures collide. Cast the lambda explicitly: `(Action)(() => ...)`. (`(TestDelegate)(...)` also compiles but is itself marked `[Obsolete]` — use `Action`.)

If a repo already targets NUnit 4 (check the `.csproj` before assuming this migration is needed — not every repo in this org is still on NUnit 3), this step is a no-op.

### 4. Local verification checklist

```
dotnet restore
dotnet build            # 0 warnings / 0 errors is the bar — CI treats this repo's build as the test gate
dotnet test             # run the full suite, confirm N/N passing, not just "build succeeded"
dotnet list package --outdated   # run again after all bumps — should report nothing left outdated
                                   # (aside from anything intentionally skipped, see section 2)
```

### 5. Committing and PR workflow

- One commit per dependabot group/category (section 1), each with build+test passing before moving to the next.
- Branch from `origin/master` (single fork/remote setup here, simpler than `osu-framework-microphone`), push, `gh pr create --repo andy840119/osu-framework-font --base master --head <branch>`.
- If CI doesn't run on the PR, check `gh api repos/<owner>/<repo>/actions/permissions` — Actions can simply be disabled on a personal fork (`"enabled": false`), which is a repo-settings problem, not a workflow-file problem. Confirm the workflow YAML itself is correct before concluding CI is broken.
