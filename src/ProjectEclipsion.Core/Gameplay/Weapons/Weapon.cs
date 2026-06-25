using System;

namespace ProjectEclipsion.Core.Gameplay.Weapons;

public sealed class Weapon
{
    public Weapon(WeaponCategory category, WeaponStats stats)
    {
        Category = category;
        Stats = stats ?? throw new ArgumentNullException(nameof(stats));
    }

    public WeaponCategory Category { get; }

    public WeaponStats Stats { get; }

    public Bullet Fire(int x, int y, int directionX, int directionY)
    {
        return new Bullet(
            BulletType.Normal,
            x,
            y,
            directionX,
            directionY,
            Stats.BulletSpeed,
            Stats.Damage);
    }
}
