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

    public BulletType BulletType => GetBulletType();

    public Bullet Fire(int x, int y, int directionX, int directionY)
    {
        var bulletType = GetBulletType();

        return new Bullet(
            bulletType,
            x,
            y,
            directionX,
            directionY,
            Stats.BulletSpeed,
            Stats.Damage,
            explosionRadius: GetExplosionRadius(bulletType),
            pierceCount: GetPierceCount(bulletType),
            chainCount: GetChainCount(bulletType),
            damageOverTime: GetDamageOverTime(bulletType),
            canHome: bulletType == BulletType.Homing);
    }

    private BulletType GetBulletType()
    {
        return Category switch
        {
            WeaponCategory.Sniper => BulletType.Laser,
            WeaponCategory.Beam => BulletType.Laser,
            WeaponCategory.Rocket => BulletType.Explosive,
            WeaponCategory.Drone => BulletType.Homing,
            _ => BulletType.Normal,
        };
    }

    private static int GetExplosionRadius(BulletType bulletType)
    {
        return bulletType == BulletType.Explosive ? 2 : 0;
    }

    private static int GetPierceCount(BulletType bulletType)
    {
        return bulletType == BulletType.Laser ? 3 : 0;
    }

    private static int GetChainCount(BulletType bulletType)
    {
        return bulletType == BulletType.Chain ? 3 : 0;
    }

    private static int GetDamageOverTime(BulletType bulletType)
    {
        return bulletType == BulletType.Plasma ? 2 : 0;
    }
}
