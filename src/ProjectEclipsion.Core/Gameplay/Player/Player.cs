
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

    public bool IsDead => Stats.IsDead;


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

    public void TakeDamage(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "ダメージ量は0以上である必要があります。");
        }

        var remainingDamage = amount;
        if (Stats.Shield > 0)
        {
            var shieldDamage = Math.Min(Stats.Shield, remainingDamage);
            Stats.SetShield(Stats.Shield - shieldDamage);
            remainingDamage -= shieldDamage;
        }

        if (remainingDamage > 0)
        {
            Stats.SetHealth(Stats.Health - remainingDamage);
        }
    }
}
