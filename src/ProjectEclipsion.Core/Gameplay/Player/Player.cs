
using System;

namespace ProjectEclipsion.Core.Gameplay.Player;

public sealed class Player
{
    public Player(PlayerStats stats, int x = 0, int y = 0)
    {
        Stats = stats ?? throw new ArgumentNullException(nameof(stats));
        X = x;
        Y = y;
    }

    public PlayerStats Stats { get; }

    public int X { get; private set; }

    public int Y { get; private set; }

    public void Move(int directionX, int directionY)
    {
        X += directionX * Stats.MoveSpeed;
        Y += directionY * Stats.MoveSpeed;
    }

    public void SetHealth(int value)
    {
        Stats.SetHealth(value);
    }

    public void RestoreHealth(int amount)
    {
        Stats.RestoreHealth(amount);
    }

    public void SetShield(int value)
    {
        Stats.SetShield(value);
    }

    public void RestoreShield(int amount)
    {
        Stats.RestoreShield(amount);
    }
}
