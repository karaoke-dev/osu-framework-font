// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Testing;

namespace osu.Framework.Font.Tests.Visual;

/// <summary>
/// All the test scene with resource should inherit this class.
/// </summary>
public abstract partial class FrameworkTestScene : TestScene
{
    /// <summary>
    /// This runner will be created only if run the unit-test.
    /// </summary>
    /// <returns></returns>
    protected override ITestSceneTestRunner CreateRunner() => new FrameworkTestSceneTestRunner();
}
