using System;

namespace ProjectEclipsion.Core.Gameplay.Weapons;

public sealed class Weapon
{
    public Weapon(WeaponCategory category, WeaponStats stats)
        : this(category.ToString(), category, stats)
    {
    }

    public Weapon(string name, WeaponCategory category, WeaponStats stats)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("武器名は空にできません。", nameof(name));
        }

        Name = name;
        Category = category;
        Stats = stats ?? throw new ArgumentNullException(nameof(stats));
    }

    public string Name { get; }

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
