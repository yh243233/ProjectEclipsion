using System;
using System.Linq;
using ProjectEclipsion.Core;
using ProjectEclipsion.Core.Gameplay.Skills;
using ProjectEclipsion.Core.Gameplay.World.Maps;
using ProjectEclipsion.Core.Rendering;

namespace ProjectEclipsion.App.Rendering;

public sealed class ConsoleRenderer : IRenderer
{
    public void Render(GameState gameState)
    {
        ArgumentNullException.ThrowIfNull(gameState);

        Console.WriteLine(gameState.Title);
        Console.WriteLine($"Player座標: X={gameState.Player.X}, Y={gameState.Player.Y}");
        Console.WriteLine($"HP: {gameState.Player.Stats.Health}/{gameState.Player.Stats.MaxHealth}");
        Console.WriteLine($"Shield: {gameState.Player.Stats.Shield}/{gameState.Player.Stats.MaxShield}");
        Console.WriteLine($"IsDead: {gameState.Player.IsDead}");
        Console.WriteLine($"Score: {gameState.Score}");
        Console.WriteLine($"SkillPoint: {gameState.Player.SkillPoint}");
        Console.WriteLine($"Combat Skills: {FormatUnlockedSkills(gameState.CombatSkillTree)}");
        Console.WriteLine($"Tech Skills: {FormatUnlockedSkills(gameState.TechSkillTree)}");
        Console.WriteLine($"Survival Skills: {FormatUnlockedSkills(gameState.SurvivalSkillTree)}");
        Console.WriteLine($"Current Room: {gameState.GameMap.CurrentRoom.Name}");
        Console.WriteLine($"Biome: {gameState.GameMap.CurrentRoom.BiomeType}");
        Console.WriteLine($"Room Position: ({gameState.GameMap.CurrentRoom.X}, {gameState.GameMap.CurrentRoom.Y})");
        Console.WriteLine($"EnemyCount: {gameState.GameMap.CurrentRoom.EnemyCount}");
        Console.WriteLine($"TreasureChestCount: {gameState.GameMap.CurrentRoom.TreasureChestCount}");
        Console.WriteLine($"Exits: {FormatExits(gameState.GameMap.CurrentRoom)}");
        if (gameState.GameMap.IsMiniMapVisible)
        {
            RenderMiniMap(gameState.GameMap);
        }

        Console.WriteLine($"Weapon: {gameState.CurrentWeapon.Name} / {gameState.CurrentWeapon.Category}");
        Console.WriteLine($"Damage: {gameState.CurrentWeapon.Stats.Damage}");
        Console.WriteLine($"FireRate: {gameState.CurrentWeapon.Stats.FireRate:0.0}");
        Console.WriteLine($"ReloadTime: {gameState.CurrentWeapon.Stats.ReloadTime:0.0}");
        Console.WriteLine($"BulletSpeed: {gameState.CurrentWeapon.Stats.BulletSpeed:0.0}");
        Console.WriteLine($"Inventory: {gameState.Inventory.Count} item(s)");
        if (gameState.Equipment.EquippedItem is null)
        {
            Console.WriteLine("Equipped Item: None");
        }
        else
        {
            Console.WriteLine($"Equipped Item: {gameState.Equipment.EquippedItem.Name} / {gameState.Equipment.EquippedItem.Rarity}");
        }

        foreach (var item in gameState.DroppedItems)
        {
            Console.WriteLine($"Dropped Item: {item.Name} / {item.Rarity}");
        }

        foreach (var bullet in gameState.Bullets)
        {
            Console.WriteLine($"Bullet: ({bullet.X}, {bullet.Y}) Type: {bullet.Type} Damage: {bullet.Damage} Speed: {bullet.Speed}");
        }

        foreach (var enemy in gameState.Enemies)
        {
            Console.WriteLine($"Enemy: ({enemy.X}, {enemy.Y}) HP: {enemy.Health} State: {enemy.AiState} Status: {FormatStatusEffects(enemy)}");
        }
    }

    private static string FormatStatusEffects(ProjectEclipsion.Core.Gameplay.Enemies.Enemy enemy)
    {
        if (enemy.StatusEffects.Count == 0)
        {
            return "None";
        }

        return string.Join(", ", enemy.StatusEffects.Effects.Select(effect => $"{effect.Type}({effect.Duration})"));
    }

    private static string FormatUnlockedSkills(SkillTree skillTree)
    {
        if (skillTree.UnlockedNodes.Count == 0)
        {
            return "None";
        }

        return string.Join(", ", skillTree.UnlockedNodes.Select(node => node.Name));
    }

    private static string FormatExits(ProjectEclipsion.Core.Gameplay.World.Rooms.Room room)
    {
        if (room.Connections.Count == 0)
        {
            return "None";
        }

        return string.Join(", ", room.Connections.Keys.OrderBy(direction => direction.ToString()));
    }

    private static void RenderMiniMap(GameMap gameMap)
    {
        var minX = gameMap.Rooms.Min(room => room.X);
        var maxX = gameMap.Rooms.Max(room => room.X);
        var minY = gameMap.Rooms.Min(room => room.Y);
        var maxY = gameMap.Rooms.Max(room => room.Y);

        Console.WriteLine("MiniMap:");
        for (var y = minY; y <= maxY; y++)
        {
            var cells = Enumerable.Range(minX, maxX - minX + 1)
                .Select(x => FormatMiniMapCell(gameMap, x, y));
            Console.WriteLine(string.Join(" ", cells));
        }

        Console.WriteLine("Legend: P=Current, V=Visited, ?=Unvisited, blank=No room");
    }

    private static string FormatMiniMapCell(GameMap gameMap, int x, int y)
    {
        var room = gameMap.FindRoomAt(x, y);
        if (room is null)
        {
            return "[ ]";
        }

        if (room == gameMap.CurrentRoom)
        {
            return "[P]";
        }

        return room.IsVisited ? "[V]" : "[?]";
    }
}
