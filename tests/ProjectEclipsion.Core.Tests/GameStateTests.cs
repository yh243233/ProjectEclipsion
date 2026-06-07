using ProjectEclipsion.Core;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class GameStateTests
{
    [Fact]
    public void 作成時にタイトルを保持する()
    {
        var gameState = new GameState();

        Assert.Equal("Project Eclipsion", gameState.Title);
    }

    [Fact]
    public void 作成時にPlayerを保持する()
    {
        var gameState = new GameState();

        Assert.NotNull(gameState.Player);
        Assert.Equal(0, gameState.Player.X);
        Assert.Equal(0, gameState.Player.Y);
    }

    [Fact]
    public void 作成時にPlayerのHPとShieldを初期化する()
    {
        var gameState = new GameState();

        Assert.Equal(100, gameState.Player.Stats.MaxHealth);
        Assert.Equal(100, gameState.Player.Stats.Health);
        Assert.Equal(50, gameState.Player.Stats.MaxShield);
        Assert.Equal(50, gameState.Player.Stats.Shield);
    }

    [Fact]
    public void MovePlayer_GameState経由でPlayerを移動する()
    {
        var gameState = new GameState();

        gameState.MovePlayer(directionX: 1, directionY: -1);

        Assert.Equal(1, gameState.Player.X);
        Assert.Equal(-1, gameState.Player.Y);
    }

    [Fact]
    public void DamagePlayer_GameState経由でPlayerにダメージを与える()
    {
        var gameState = new GameState();

        gameState.DamagePlayer(10);

        Assert.Equal(100, gameState.Player.Stats.Health);
        Assert.Equal(40, gameState.Player.Stats.Shield);
        Assert.False(gameState.Player.IsDead);
    }
}
