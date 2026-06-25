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
    public void 作成時にScoreは0である()
    {
        var gameState = new GameState();

        Assert.Equal(0, gameState.Score);
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
    public void 作成時に複数武器を保持する()
    {
        var gameState = new GameState();

        Assert.Equal(6, gameState.Weapons.Count);
        Assert.Contains(gameState.Weapons, weapon => weapon.Category == WeaponCategory.Assault);
        Assert.Contains(gameState.Weapons, weapon => weapon.Category == WeaponCategory.Shotgun);
        Assert.Contains(gameState.Weapons, weapon => weapon.Category == WeaponCategory.Sniper);
        Assert.Contains(gameState.Weapons, weapon => weapon.Category == WeaponCategory.Beam);
        Assert.Contains(gameState.Weapons, weapon => weapon.Category == WeaponCategory.Rocket);
        Assert.Contains(gameState.Weapons, weapon => weapon.Category == WeaponCategory.Drone);
    }

    [Theory]
    [InlineData(1, WeaponCategory.Assault)]
    [InlineData(2, WeaponCategory.Shotgun)]
    [InlineData(3, WeaponCategory.Sniper)]
    [InlineData(4, WeaponCategory.Beam)]
    [InlineData(5, WeaponCategory.Rocket)]
    [InlineData(6, WeaponCategory.Drone)]
    public void SwitchWeapon_番号で現在武器を切り替える(int weaponNumber, WeaponCategory expectedCategory)
    {
        var gameState = new GameState();

        gameState.SwitchWeapon(weaponNumber);

        Assert.Equal(expectedCategory, gameState.CurrentWeapon.Category);
    }

    [Fact]
    public void SwitchWeapon_無効な番号では現在武器を変更しない()
    {
        var gameState = new GameState();
        gameState.SwitchWeapon(3);

        gameState.SwitchWeapon(7);

        Assert.Equal(WeaponCategory.Sniper, gameState.CurrentWeapon.Category);
    }

    [Fact]
    public void SwitchWeapon_Shotgunへ切り替えできる()
    {
        var gameState = new GameState();

        gameState.SwitchWeapon(2);

        Assert.Equal(WeaponCategory.Shotgun, gameState.CurrentWeapon.Category);
    }

    [Fact]
    public void SwitchWeapon_Sniperへ切り替えできる()
    {
        var gameState = new GameState();

        gameState.SwitchWeapon(3);

        Assert.Equal(WeaponCategory.Sniper, gameState.CurrentWeapon.Category);
    }

    [Fact]
    public void SwitchWeapon_Beamへ切り替えできる()
    {
        var gameState = new GameState();

        gameState.SwitchWeapon(4);

        Assert.Equal(WeaponCategory.Beam, gameState.CurrentWeapon.Category);
    }

    [Fact]
    public void SwitchWeapon_Rocketへ切り替えできる()
    {
        var gameState = new GameState();

        gameState.SwitchWeapon(5);

        Assert.Equal(WeaponCategory.Rocket, gameState.CurrentWeapon.Category);
    }

    [Fact]
    public void SwitchWeapon_Droneへ切り替えできる()
    {
        var gameState = new GameState();

        gameState.SwitchWeapon(6);

        Assert.Equal(WeaponCategory.Drone, gameState.CurrentWeapon.Category);
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
    public void FireCurrentWeapon_現在武器でBulletを発射する()
    {
        var gameState = new GameState();
        gameState.SwitchWeapon(3);

        gameState.FireCurrentWeapon();

        Assert.Single(gameState.Bullets);
        Assert.Equal(25, gameState.Bullets[0].Damage);
        Assert.Equal(3, gameState.Bullets[0].Speed);
    }

    [Fact]
    public void FireCurrentWeapon_武器ごとにBulletのDamageが異なる()
    {
        var gameState = new GameState();
        gameState.SwitchWeapon(1);
        gameState.FireCurrentWeapon();
        var assaultDamage = gameState.Bullets[0].Damage;

        gameState.Bullets.Clear();
        gameState.SwitchWeapon(5);
        gameState.FireCurrentWeapon();
        var rocketDamage = gameState.Bullets[0].Damage;

        Assert.NotEqual(assaultDamage, rocketDamage);
        Assert.Equal(10, assaultDamage);
        Assert.Equal(30, rocketDamage);
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

    [Fact]
    public void Update_Enemy撃破時にScoreを100加算する()
    {
        var gameState = new GameState();
        gameState.Enemies[0].TakeDamage(20);
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: 28, y: 9, directionX: 1, directionY: 0, speed: 1, damage: 10));

        gameState.Update();

        Assert.Equal(100, gameState.Score);
    }

    [Fact]
    public void Update_Enemyにダメージを与えただけではScoreは増えない()
    {
        var gameState = new GameState();
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: 28, y: 9, directionX: 1, directionY: 0, speed: 1, damage: 10));

        gameState.Update();

        Assert.Equal(0, gameState.Score);
        Assert.Single(gameState.Enemies);
        Assert.Equal(20, gameState.Enemies[0].Health);
    }

    [Fact]
    public void Update_死亡したEnemyが削除されてもScoreは維持される()
    {
        var gameState = new GameState();
        gameState.Enemies[0].TakeDamage(20);
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: 28, y: 9, directionX: 1, directionY: 0, speed: 1, damage: 10));
        gameState.Update();

        gameState.Update();

        Assert.Equal(100, gameState.Score);
        Assert.Empty(gameState.Enemies);
    }

    [Fact]
    public void Update_同じEnemy撃破でScoreを二重加算しない()
    {
        var gameState = new GameState();
        gameState.Enemies[0].TakeDamage(20);
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: 28, y: 9, directionX: 1, directionY: 0, speed: 1, damage: 10));
        gameState.Bullets.Add(new Bullet(BulletType.Normal, x: 28, y: 9, directionX: 1, directionY: 0, speed: 1, damage: 10));

        gameState.Update();

        Assert.Equal(100, gameState.Score);
        Assert.Empty(gameState.Enemies);
    }
}
