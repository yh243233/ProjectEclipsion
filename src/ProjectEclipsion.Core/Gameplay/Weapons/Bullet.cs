namespace ProjectEclipsion.Core.Gameplay.Weapons;

public sealed class Bullet
{
    public Bullet(
        BulletType type,
        int x,
        int y,
        int directionX,
        int directionY,
        int speed,
        int damage,
        int explosionRadius = 0,
        int pierceCount = 0,
        int chainCount = 0,
        int damageOverTime = 0,
        bool canHome = false)
    {
        Type = type;
        X = x;
        Y = y;
        DirectionX = directionX;
        DirectionY = directionY;
        Speed = speed;
        Damage = damage;
        ExplosionRadius = explosionRadius;
        PierceCount = pierceCount;
        ChainCount = chainCount;
        DamageOverTime = damageOverTime;
        CanHome = canHome;
        HomingTargetX = null;
        HomingTargetY = null;
        IsActive = true;
    }

    public BulletType Type { get; }

    public int X { get; private set; }

    public int Y { get; private set; }

    public int DirectionX { get; private set; }

    public int DirectionY { get; private set; }

    public int Speed { get; }

    public int Damage { get; }

    public bool IsActive { get; private set; }

    public int ExplosionRadius { get; }

    public int PierceCount { get; private set; }

    public int ChainCount { get; }

    public int DamageOverTime { get; }

    public bool CanHome { get; }

    public int? HomingTargetX { get; private set; }

    public int? HomingTargetY { get; private set; }

    public bool HasHomingTarget => HomingTargetX.HasValue && HomingTargetY.HasValue;

    public void Update()
    {
        if (!IsActive)
        {
            return;
        }

        X += DirectionX * Speed;
        Y += DirectionY * Speed;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void SetDirection(int directionX, int directionY)
    {
        DirectionX = directionX;
        DirectionY = directionY;
    }

    public void SetHomingTarget(int x, int y)
    {
        HomingTargetX = x;
        HomingTargetY = y;
    }

    public bool TryConsumePierce()
    {
        if (PierceCount <= 0)
        {
            return false;
        }

        PierceCount--;
        return true;
    }
}
