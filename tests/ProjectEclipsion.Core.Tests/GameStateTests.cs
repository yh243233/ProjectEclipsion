using ProjectEclipsion.Core;
using ProjectEclipsion.Core.Gameplay.Enemies;
using ProjectEclipsion.Core.Gameplay.Weapons;
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

    [Fact]
    public void 作成時にAssault武器を保持する()
    {
        var gameState = new GameState();

        Assert.Equal(WeaponCategory.Assault, gameState.CurrentWeapon.Category);
    }

    [Fact]
    public void FireCurrentWeapon_GameState経由でBulletを発射する()
    {
        var gameState = new GameState();

        gameState.FireCurrentWeapon();

        Assert.Single(gameState.Bullets);
        Assert.Equal(gameState.Player.X, gameState.Bullets[0].X);
        Assert.Equal(gameState.Player.Y, gameState.Bullets[0].Y);
        Assert.Equal(BulletType.Normal, gameState.Bullets[0].Type);
    }

    [Fact]
    public void Update_Bulletを移動する()
    {
        var gameState = new GameState();
        gameState.FireCurrentWeapon();
        var startX = gameState.Bullets[0].X;

        gameState.Update();

        Assert.Equal(startX + 1, gameState.Bullets[0].X);
        Assert.Equal(gameState.Player.Y, gameState.Bullets[0].Y);
    }

    [Fact]
    public void 作成時にEnemyを保持する()
    {
        var gameState = new GameState();

        Assert.Single(gameState.Enemies);
        Assert.Equal(EnemyAiLevel.Basic, gameState.Enemies[0].AiLevel);
    }

    [Fact]
    public void Update_EnemyをPlayerへ接近させる()
    {
        var gameState = new GameState();
        var enemy = gameState.Enemies[0];
        var startX = enemy.X;
        var startY = enemy.Y;

        gameState.Update();

        Assert.Equal(startX - 1, enemy.X);
        Assert.Equal(startY - 1, enemy.Y);
        Assert.Equal(EnemyAiState.Combat, enemy.AiState);
    }

    [Fact]
    public void Update_BulletとEnemyの座標が一致したらEnemyのHPが減る()
    {
        var gameState = new GameState();
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: 28, y: 9, directionX: 1, directionY: 0, speed: 1, damage: 10));

        gameState.Update();

        Assert.Single(gameState.Enemies);
        Assert.Equal(20, gameState.Enemies[0].Health);
    }

    [Fact]
    public void Update_Bullet命中後にBulletが削除される()
    {
        var gameState = new GameState();
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: 28, y: 9, directionX: 1, directionY: 0, speed: 1, damage: 10));

        gameState.Update();

        Assert.Empty(gameState.Bullets);
    }

    [Fact]
    public void Update_非アクティブなBulletを削除する()
    {
        var gameState = new GameState();
        var bullet = new Bullet(BulletType.Normal, x: 1, y: 0, directionX: 1, directionY: 0, speed: 1, damage: 10);
        bullet.Deactivate();
        gameState.Bullets.Add(bullet);

        gameState.Update();

        Assert.Empty(gameState.Bullets);
    }

    [Fact]
    public void Update_死亡したEnemyを削除する()
    {
        var gameState = new GameState();
        gameState.Enemies[0].TakeDamage(30);

        gameState.Update();

        Assert.Empty(gameState.Enemies);
    }

    [Fact]
    public void Update_BulletとEnemyの座標が一致しない場合はEnemyのHPが減らない()
    {
        var gameState = new GameState();
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: 1, y: 0, directionX: 1, directionY: 0, speed: 1, damage: 10));

        gameState.Update();

        Assert.Single(gameState.Enemies);
        Assert.Equal(30, gameState.Enemies[0].Health);
    }

    [Fact]
    public void Update_Bullet命中でEnemyのHPが0になるとEnemyを削除する()
    {
        var gameState = new GameState();
        gameState.Enemies[0].TakeDamage(20);
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: 28, y: 9, directionX: 1, directionY: 0, speed: 1, damage: 10));

        gameState.Update();

        Assert.Empty(gameState.Enemies);
        Assert.Empty(gameState.Bullets);
    }
}
