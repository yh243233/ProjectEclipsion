using System;
using System.Collections.Generic;
using System.Linq;
using ProjectEclipsion.Core;
using ProjectEclipsion.Core.Gameplay.Skills;

namespace ProjectEclipsion.App.Rendering;

public sealed class HudRenderer
{
    private const int BarWidth = 10;

    public void Render(GameState gameState)
    {
        ArgumentNullException.ThrowIfNull(gameState);

        foreach (var line in BuildLines(gameState))
        {
            Console.WriteLine(line);
        }
    }

    public IReadOnlyList<string> BuildLines(GameState gameState)
    {
        ArgumentNullException.ThrowIfNull(gameState);

        var stats = gameState.Player.Stats;
        var lines = new List<string>
        {
            FormatBar("HP", stats.Health, stats.MaxHealth),
            FormatBar("Shield", stats.Shield, stats.MaxShield),
            FormatBar("Energy", stats.Energy, stats.MaxEnergy),
            $"Weapon: {gameState.CurrentWeapon.Name}",
            $"Category: {gameState.CurrentWeapon.Category}",
            $"Damage: {gameState.CurrentWeapon.Stats.Damage}",
            $"BulletType: {gameState.CurrentWeapon.BulletType}",
            $"Score: {gameState.Score}",
            "Reload: Ready",
        };

        lines.AddRange(BuildSkillCooldownLines(gameState.CombatSkillTree, gameState.TechSkillTree, gameState.SurvivalSkillTree));
        return lines;
    }

    public static double CalculateRatio(int current, int max)
    {
        if (max <= 0)
        {
            return 0;
        }

        return Math.Clamp((double)current / max, 0, 1);
    }

    public static string FormatBar(string label, int current, int max)
    {
        var ratio = CalculateRatio(current, max);
        var filledCount = (int)Math.Round(ratio * BarWidth, MidpointRounding.AwayFromZero);
        var emptyCount = BarWidth - filledCount;
        return $"{label,-7}[{new string('#', filledCount)}{new string('-', emptyCount)}] {current}/{max}";
    }

    public static string FormatCooldown(int cooldown)
    {
        return cooldown <= 0 ? "Ready" : cooldown.ToString();
    }

    private static IReadOnlyList<string> BuildSkillCooldownLines(params SkillTree[] skillTrees)
    {
        var unlockedSkills = skillTrees
            .SelectMany(tree => tree.UnlockedNodes)
            .ToList();

        var lines = new List<string> { "Skills:" };
        if (unlockedSkills.Count == 0)
        {
            lines.Add("- None");
            return lines;
        }

        foreach (var skill in unlockedSkills)
        {
            lines.Add($"- {skill.Name} CD: {FormatCooldown(skill.Cooldown)}");
        }

        return lines;
    }
}
