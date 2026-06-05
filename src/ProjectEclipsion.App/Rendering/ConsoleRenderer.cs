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
    }
}
