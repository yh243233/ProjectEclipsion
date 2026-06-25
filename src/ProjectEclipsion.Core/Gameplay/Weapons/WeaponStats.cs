namespace ProjectEclipsion.Core.Gameplay.Weapons;

public sealed class WeaponStats
{
    public WeaponStats(int damage, int bulletSpeed, double fireRate = 1.0, double reloadTime = 1.0)
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
        FireRate = fireRate;
        ReloadTime = reloadTime;
    }

    public int Damage { get; }

    public int BulletSpeed { get; }

    public double FireRate { get; }

    public double ReloadTime { get; }
}
