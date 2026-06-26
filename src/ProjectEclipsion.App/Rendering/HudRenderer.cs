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

        var stats = gameState.Player.Stats;
        Console.WriteLine(FormatBar("HP", stats.Health, stats.MaxHealth));
        Console.WriteLine(FormatBar("Shield", stats.Shield, stats.MaxShield));
        Console.WriteLine(FormatBar("Energy", stats.Energy, stats.MaxEnergy));
        Console.WriteLine($"Weapon: {gameState.CurrentWeapon.Name} / {gameState.CurrentWeapon.Category}");
        Console.WriteLine($"Damage: {gameState.CurrentWeapon.Stats.Damage}");
        Console.WriteLine($"BulletType: {gameState.CurrentWeapon.BulletType}");
        Console.WriteLine("Reload: Ready");
        RenderDamageLog(gameState.RecentDamageLogs);
        RenderSkillCooldowns(gameState.CombatSkillTree, gameState.TechSkillTree, gameState.SurvivalSkillTree);
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

    private static void RenderDamageLog(IReadOnlyList<string> damageLogs)
    {
        Console.WriteLine("Damage Log:");
        if (damageLogs.Count == 0)
        {
            Console.WriteLine("- None");
            return;
        }

        foreach (var log in damageLogs)
        {
            Console.WriteLine($"- {log}");
        }
    }

    private static void RenderSkillCooldowns(params SkillTree[] skillTrees)
    {
        var unlockedSkills = skillTrees
            .SelectMany(tree => tree.UnlockedNodes)
            .ToList();

        Console.WriteLine("Skills:");
        if (unlockedSkills.Count == 0)
        {
            Console.WriteLine("- None");
            return;
        }

        foreach (var skill in unlockedSkills)
        {
            Console.WriteLine($"- {skill.Name} CD: {FormatCooldown(skill.Cooldown)}");
        }
    }
}
