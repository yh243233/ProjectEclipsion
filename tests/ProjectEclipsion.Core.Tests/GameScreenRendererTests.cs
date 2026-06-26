using System.Linq;
using ProjectEclipsion.App.Rendering;
using ProjectEclipsion.Core;
using ProjectEclipsion.Core.Gameplay.Enemies;
using ProjectEclipsion.Core.Gameplay.Weapons;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class GameScreenRendererTests
{
    [Fact]
    public void BuildLines_簡易マップにPlayerを配置できる()
    {
        var renderer = new GameScreenRenderer();
        var gameState = new GameState();

        var lines = renderer.BuildLines(gameState, width: 9, height: 5);

        Assert.Equal('P', lines[2][4]);
    }

    [Fact]
    public void BuildLines_簡易マップにEnemyを配置できる()
    {
        var renderer = new GameScreenRenderer();
        var gameState = new GameState();
        gameState.Enemies.Clear();
        gameState.Enemies.Add(new Enemy(x: 1, y: 0, maxHealth: 10, aiLevel: EnemyAiLevel.Basic));

        var lines = renderer.BuildLines(gameState, width: 9, height: 5);

        Assert.Equal('E', lines[2][5]);
    }

    [Fact]
    public void BuildLines_簡易マップにBulletを配置できる()
    {
        var renderer = new GameScreenRenderer();
        var gameState = new GameState();
        gameState.Enemies.Clear();
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: 1, y: 0, directionX: 1, directionY: 0, speed: 1, damage: 10));

        var lines = renderer.BuildLines(gameState, width: 9, height: 5);

        Assert.Equal('*', lines[2][5]);
    }

    [Fact]
    public void BuildLines_範囲外座標を無視して例外が出ない()
    {
        var renderer = new GameScreenRenderer();
        var gameState = new GameState();
        gameState.Enemies.Clear();
        gameState.Enemies.Add(new Enemy(x: 999, y: 999, maxHealth: 10, aiLevel: EnemyAiLevel.Basic));
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: -999, y: -999, directionX: 1, directionY: 0, speed: 1, damage: 10));

        var lines = renderer.BuildLines(gameState, width: 9, height: 5);

        Assert.Equal(5, lines.Count);
        Assert.Contains(lines, line => line.Contains('P'));
        Assert.DoesNotContain(lines, line => line.Contains('E'));
        Assert.DoesNotContain(lines, line => line.Contains('*'));
        Assert.True(lines.All(line => line.Length == 9));
    }
}
