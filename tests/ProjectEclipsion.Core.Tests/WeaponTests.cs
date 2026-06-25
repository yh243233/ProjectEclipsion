using ProjectEclipsion.Core.Gameplay.Weapons;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class WeaponTests
{
    [Fact]
    public void Fire_Bulletを生成する()
    {
        var weapon = new Weapon(WeaponCategory.Assault, new WeaponStats(damage: 10, bulletSpeed: 1));

        var bullet = weapon.Fire(x: 5, y: 7, directionX: 1, directionY: 0);

        Assert.NotNull(bullet);
    }

    [Fact]
    public void Fire_Bulletの初期座標が発射座標と一致する()
    {
        var weapon = new Weapon(WeaponCategory.Assault, new WeaponStats(damage: 10, bulletSpeed: 1));

        var bullet = weapon.Fire(x: 5, y: 7, directionX: 1, directionY: 0);

        Assert.Equal(5, bullet.X);
        Assert.Equal(7, bullet.Y);
    }

    [Fact]
    public void Fire_BulletTypeはNormalになる()
    {
        var weapon = new Weapon(WeaponCategory.Assault, new WeaponStats(damage: 10, bulletSpeed: 1));

        var bullet = weapon.Fire(x: 5, y: 7, directionX: 1, directionY: 0);

        Assert.Equal(BulletType.Normal, bullet.Type);
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
