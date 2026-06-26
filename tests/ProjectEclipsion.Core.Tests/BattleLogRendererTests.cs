using System.Collections.Generic;
using ProjectEclipsion.App.Rendering;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class BattleLogRendererTests
{
    [Fact]
    public void BuildLines_戦闘ログが最大5件に制限される()
    {
        var renderer = new BattleLogRenderer();
        var logs = new List<string>
        {
            "Log 1",
            "Log 2",
            "Log 3",
            "Log 4",
            "Log 5",
            "Log 6",
        };

        var lines = renderer.BuildLines(logs);

        Assert.Equal(5, lines.Count);
        Assert.DoesNotContain("Log 1", lines);
        Assert.Contains("Log 6", lines);
    }

    [Fact]
    public void BuildLines_ログが存在しない場合でも表示が崩れない()
    {
        var renderer = new BattleLogRenderer();

        var lines = renderer.BuildLines(new List<string>());

        Assert.Single(lines);
        Assert.Equal("- None", lines[0]);
    }
}
