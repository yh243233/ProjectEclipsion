using System.Collections.Generic;
using System.Linq;

namespace ProjectEclipsion.App.Rendering;

public sealed class BattleLogRenderer
{
    public const int MaxLogCount = 5;

    public IReadOnlyList<string> BuildLines(IReadOnlyList<string> logs)
    {
        var recentLogs = logs
            .TakeLast(MaxLogCount)
            .ToList();

        if (recentLogs.Count == 0)
        {
            return new[] { "- None" };
        }

        return recentLogs;
    }
}
