namespace ProjectEclipsion.Core.Gameplay.Weapons;

public sealed class WeaponStats
{
    public WeaponStats(int damage, int bulletSpeed)
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
    }

    public int Damage { get; }

    public int BulletSpeed { get; }
}
