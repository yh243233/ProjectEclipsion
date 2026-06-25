using ProjectEclipsion.Core.Gameplay.Weapons;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class WeaponTests
{
    [Fact]
    public void Fire_Bulletを生成する()
    {
        var weapon = new Weapon("Starter Assault", WeaponCategory.Assault, new WeaponStats(damage: 10, bulletSpeed: 1));

        var bullet = weapon.Fire(x: 5, y: 7, directionX: 1, directionY: 0);

        Assert.NotNull(bullet);
    }

    [Fact]
    public void Fire_Bulletの初期座標が発射座標と一致する()
    {
        var weapon = new Weapon("Starter Assault", WeaponCategory.Assault, new WeaponStats(damage: 10, bulletSpeed: 1));

        var bullet = weapon.Fire(x: 5, y: 7, directionX: 1, directionY: 0);

        Assert.Equal(5, bullet.X);
        Assert.Equal(7, bullet.Y);
    }

    [Fact]
    public void Fire_BulletTypeはNormalになる()
    {
        var weapon = new Weapon("Starter Assault", WeaponCategory.Assault, new WeaponStats(damage: 10, bulletSpeed: 1));

        var bullet = weapon.Fire(x: 5, y: 7, directionX: 1, directionY: 0);

        Assert.Equal(BulletType.Normal, bullet.Type);
    }

    [Fact]
    public void BulletType_拡張弾タイプが存在する()
    {
        Assert.True(System.Enum.IsDefined(typeof(BulletType), BulletType.Explosive));
        Assert.True(System.Enum.IsDefined(typeof(BulletType), BulletType.Laser));
        Assert.True(System.Enum.IsDefined(typeof(BulletType), BulletType.Homing));
        Assert.True(System.Enum.IsDefined(typeof(BulletType), BulletType.Chain));
        Assert.True(System.Enum.IsDefined(typeof(BulletType), BulletType.Plasma));
    }

    [Theory]
    [InlineData(WeaponCategory.Assault, BulletType.Normal)]
    [InlineData(WeaponCategory.Shotgun, BulletType.Normal)]
    [InlineData(WeaponCategory.Sniper, BulletType.Laser)]
    [InlineData(WeaponCategory.Beam, BulletType.Laser)]
    [InlineData(WeaponCategory.Rocket, BulletType.Explosive)]
    [InlineData(WeaponCategory.Drone, BulletType.Homing)]
    public void Fire_WeaponCategoryに応じたBulletTypeを生成する(WeaponCategory category, BulletType expectedType)
    {
        var weapon = new Weapon(category.ToString(), category, new WeaponStats(damage: 10, bulletSpeed: 1));

        var bullet = weapon.Fire(x: 5, y: 7, directionX: 1, directionY: 0);

        Assert.Equal(expectedType, bullet.Type);
    }

    [Fact]
    public void Fire_Explosive弾は爆発範囲値を持つ()
    {
        var weapon = new Weapon("Impact Rocket", WeaponCategory.Rocket, new WeaponStats(damage: 30, bulletSpeed: 1));

        var bullet = weapon.Fire(x: 5, y: 7, directionX: 1, directionY: 0);

        Assert.Equal(BulletType.Explosive, bullet.Type);
        Assert.True(bullet.ExplosionRadius > 0);
    }

    [Fact]
    public void Fire_Laser弾はPierceCountを持つ()
    {
        var weapon = new Weapon("Longshot Sniper", WeaponCategory.Sniper, new WeaponStats(damage: 25, bulletSpeed: 3));

        var bullet = weapon.Fire(x: 5, y: 7, directionX: 1, directionY: 0);

        Assert.Equal(BulletType.Laser, bullet.Type);
        Assert.True(bullet.PierceCount > 0);
    }

    [Fact]
    public void Fire_Homing弾は追尾情報を持てる()
    {
        var weapon = new Weapon("Support Drone", WeaponCategory.Drone, new WeaponStats(damage: 5, bulletSpeed: 2));

        var bullet = weapon.Fire(x: 5, y: 7, directionX: 1, directionY: 0);
        bullet.SetHomingTarget(x: 10, y: 3);

        Assert.Equal(BulletType.Homing, bullet.Type);
        Assert.True(bullet.CanHome);
        Assert.True(bullet.HasHomingTarget);
        Assert.Equal(10, bullet.HomingTargetX);
        Assert.Equal(3, bullet.HomingTargetY);
    }

    [Fact]
    public void Chain弾は連鎖回数を持つ()
    {
        var bullet = new Bullet(BulletType.Chain, x: 5, y: 7, directionX: 1, directionY: 0, speed: 1, damage: 10, chainCount: 3);

        Assert.Equal(BulletType.Chain, bullet.Type);
        Assert.Equal(3, bullet.ChainCount);
    }

    [Fact]
    public void Plasma弾は継続ダメージ値を持つ()
    {
        var bullet = new Bullet(BulletType.Plasma, x: 5, y: 7, directionX: 1, directionY: 0, speed: 1, damage: 10, damageOverTime: 2);

        Assert.Equal(BulletType.Plasma, bullet.Type);
        Assert.Equal(2, bullet.DamageOverTime);
    }

    [Fact]
    public void Weapon_武器名とカテゴリとステータスを保持する()
    {
        var stats = new WeaponStats(damage: 10, bulletSpeed: 1, fireRate: 3.0, reloadTime: 1.5);

        var weapon = new Weapon("Starter Assault", WeaponCategory.Assault, stats);

        Assert.Equal("Starter Assault", weapon.Name);
        Assert.Equal(WeaponCategory.Assault, weapon.Category);
        Assert.Same(stats, weapon.Stats);
        Assert.Equal(3.0, weapon.Stats.FireRate);
        Assert.Equal(1.5, weapon.Stats.ReloadTime);
    }

    [Fact]
    public void Update_右方向のBulletはX座標が増える()
    {
        var bullet = new Bullet(BulletType.Normal, x: 5, y: 7, directionX: 1, directionY: 0, speed: 1, damage: 10);

        bullet.Update();

        Assert.Equal(6, bullet.X);
        Assert.Equal(7, bullet.Y);
    }

    [Fact]
    public void Deactivate_Bulletを非アクティブにする()
    {
        var bullet = new Bullet(BulletType.Normal, x: 5, y: 7, directionX: 1, directionY: 0, speed: 1, damage: 10);

        bullet.Deactivate();

        Assert.False(bullet.IsActive);
    }

    [Fact]
    public void Update_非アクティブなBulletは移動しない()
    {
        var bullet = new Bullet(BulletType.Normal, x: 5, y: 7, directionX: 1, directionY: 0, speed: 1, damage: 10);
        bullet.Deactivate();

        bullet.Update();

        Assert.Equal(5, bullet.X);
        Assert.Equal(7, bullet.Y);
    }
}
