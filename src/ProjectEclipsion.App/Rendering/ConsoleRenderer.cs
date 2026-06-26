using System;
using System.Collections.Generic;
using System.Linq;
using ProjectEclipsion.Core;
using ProjectEclipsion.Core.Rendering;

namespace ProjectEclipsion.App.Rendering;

public sealed class ConsoleRenderer : IRenderer
{
    private const int LeftWidth = 30;
    private const int RightWidth = 31;
    private readonly HudRenderer hudRenderer = new();
    private readonly GameScreenRenderer gameScreenRenderer = new();
    private readonly BattleLogRenderer battleLogRenderer = new();
    private readonly MiniMapRenderer miniMapRenderer = new();
    private readonly InputGuideRenderer inputGuideRenderer = new();

    public void Render(GameState gameState)
    {
        ArgumentNullException.ThrowIfNull(gameState);

        if (!Console.IsOutputRedirected)
        {
            Console.SetCursorPosition(0, 0);
        }

        foreach (var line in BuildLines(gameState))
        {
            Console.WriteLine(line);
        }
    }

    public IReadOnlyList<string> BuildLines(GameState gameState)
    {
        ArgumentNullException.ThrowIfNull(gameState);

        var lines = new List<string>
        {
            Border(LeftWidth + RightWidth + 3),
            FullWidthLine(gameState.Title),
            Border(LeftWidth + RightWidth + 3),
        };

        lines.AddRange(TwoColumnSection(
            "MAP",
            gameScreenRenderer.BuildLines(gameState),
            "HUD",
            BuildHudLines(gameState),
            leftHeight: 13,
            rightHeight: 13));

        lines.AddRange(TwoColumnSection(
            "MiniMap",
            miniMapRenderer.BuildLines(gameState.GameMap),
            "Battle Log",
            battleLogRenderer.BuildLines(gameState.RecentDamageLogs),
            leftHeight: 5,
            rightHeight: 5));

        foreach (var inputGuideLine in inputGuideRenderer.BuildLines())
        {
            lines.Add(FullWidthLine(inputGuideLine));
        }

        lines.Add(Border(LeftWidth + RightWidth + 3));
        return lines;
    }

    private IReadOnlyList<string> BuildHudLines(GameState gameState)
    {
        var lines = hudRenderer.BuildLines(gameState).ToList();

        lines.Add($"Room: {gameState.GameMap.CurrentRoom.Name}");
        lines.Add($"Biome: {gameState.GameMap.CurrentRoom.BiomeType}");
        lines.Add($"EnemyCount: {gameState.GameMap.CurrentRoom.EnemyCount}");
        lines.Add($"Treasure: {gameState.GameMap.CurrentRoom.TreasureChestCount}");
        if (!string.IsNullOrWhiteSpace(gameState.SaveMessage))
        {
            lines.Add(gameState.SaveMessage);
        }

        return lines;
    }

    private static IReadOnlyList<string> TwoColumnSection(
        string leftTitle,
        IReadOnlyList<string> leftLines,
        string rightTitle,
        IReadOnlyList<string> rightLines,
        int leftHeight,
        int rightHeight)
    {
        var height = Math.Max(leftHeight, rightHeight);
        var lines = new List<string>
        {
            $"| {Fit(leftTitle, LeftWidth)} | {Fit(rightTitle, RightWidth)} |",
        };

        for (var i = 0; i < height; i++)
        {
            var left = i < leftLines.Count ? leftLines[i] : string.Empty;
            var right = i < rightLines.Count ? rightLines[i] : string.Empty;
            lines.Add($"| {Fit(left, LeftWidth)} | {Fit(right, RightWidth)} |");
        }

        lines.Add(Border(LeftWidth + RightWidth + 3));
        return lines;
    }

    private static string FullWidthLine(string text)
    {
        return $"| {Fit(text, LeftWidth + RightWidth + 3)} |";
    }

    private static string Border(int innerWidth)
    {
        return $"+{new string('-', innerWidth + 2)}+";
    }

    private static string Fit(string text, int width)
    {
        if (text.Length > width)
        {
            return text[..width];
        }

        return text.PadRight(width);
    }
}
