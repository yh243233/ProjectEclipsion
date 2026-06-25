using ProjectEclipsion.Core.Gameplay.Enemies;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class EnemyTests
{
    [Fact]
    public void 作成時にHPを最大値で初期化する()
    {
        var enemy = new Enemy(x: 30, y: 10, maxHealth: 30, aiLevel: EnemyAiLevel.Basic);

        Assert.Equal(30, enemy.MaxHealth);
        Assert.Equal(30, enemy.Health);
    }

    [Fact]
    public void 作成時にAiLevelはBasicである()
    {
        var enemy = new Enemy(x: 30, y: 10, maxHealth: 30, aiLevel: EnemyAiLevel.Basic);

        Assert.Equal(EnemyAiLevel.Basic, enemy.AiLevel);
    }

    [Fact]
    public void Update_AiStateがIdleからCombatへ変化する()
    {
        var enemy = new Enemy(x: 30, y: 10, maxHealth: 30, aiLevel: EnemyAiLevel.Basic);

        enemy.Update(playerX: 0, playerY: 10);

        Assert.Equal(EnemyAiState.Combat, enemy.AiState);
    }

    [Fact]
    public void Update_PlayerのX座標へ近づく()
    {
        var enemy = new Enemy(x: 30, y: 10, maxHealth: 30, aiLevel: EnemyAiLevel.Basic);

        enemy.Update(playerX: 0, playerY: 10);

        Assert.Equal(29, enemy.X);
        Assert.Equal(10, enemy.Y);
    }

    [Fact]
    public void Update_PlayerのY座標へ近づく()
    {
        var enemy = new Enemy(x: 30, y: 10, maxHealth: 30, aiLevel: EnemyAiLevel.Basic);

        enemy.Update(playerX: 30, playerY: 0);

        Assert.Equal(30, enemy.X);
        Assert.Equal(9, enemy.Y);
    }

    [Fact]
    public void TakeDamage_HPが減る()
    {
        var enemy = new Enemy(x: 30, y: 10, maxHealth: 30, aiLevel: EnemyAiLevel.Basic);

        enemy.TakeDamage(10);

        Assert.Equal(20, enemy.Health);
    }

    [Fact]
    public void TakeDamage_HPが0未満にならない()
    {
        var enemy = new Enemy(x: 30, y: 10, maxHealth: 30, aiLevel: EnemyAiLevel.Basic);

        enemy.TakeDamage(100);

        Assert.Equal(0, enemy.Health);
    }

    [Fact]
    public void TakeDamage_HPが0になるとIsDeadがtrueになる()
    {
        var enemy = new Enemy(x: 30, y: 10, maxHealth: 30, aiLevel: EnemyAiLevel.Basic);

        enemy.TakeDamage(30);

        Assert.True(enemy.IsDead);
    }
}
