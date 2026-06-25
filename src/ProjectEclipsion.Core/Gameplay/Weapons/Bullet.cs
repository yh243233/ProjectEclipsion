namespace ProjectEclipsion.Core.Gameplay.Weapons;

public sealed class Bullet
{
    public Bullet(BulletType type, int x, int y, int directionX, int directionY, int speed, int damage)
    {
        Type = type;
        X = x;
        Y = y;
        DirectionX = directionX;
        DirectionY = directionY;
        Speed = speed;
        Damage = damage;
    }

    public BulletType Type { get; }

    public int X { get; private set; }

    public int Y { get; private set; }

    public int DirectionX { get; }

    public int DirectionY { get; }

    public int Speed { get; }

    public int Damage { get; }

    public void Update()
    {
        X += DirectionX * Speed;
        Y += DirectionY * Speed;
    }
}
