using System;
using ProjectEclipsion.Core;
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
        Console.WriteLine($"Weapon: {gameState.CurrentWeapon.Name} / {gameState.CurrentWeapon.Category}");
        Console.WriteLine($"Damage: {gameState.CurrentWeapon.Stats.Damage}");
        Console.WriteLine($"FireRate: {gameState.CurrentWeapon.Stats.FireRate:0.0}");
        Console.WriteLine($"ReloadTime: {gameState.CurrentWeapon.Stats.ReloadTime:0.0}");
        Console.WriteLine($"BulletSpeed: {gameState.CurrentWeapon.Stats.BulletSpeed:0.0}");

        foreach (var bullet in gameState.Bullets)
        {
            Console.WriteLine($"Bullet: ({bullet.X}, {bullet.Y}) Type: {bullet.Type} Damage: {bullet.Damage} Speed: {bullet.Speed}");
        }

        foreach (var enemy in gameState.Enemies)
        {
            Console.WriteLine($"Enemy: ({enemy.X}, {enemy.Y}) HP: {enemy.Health} State: {enemy.AiState}");
        }
    }
}
