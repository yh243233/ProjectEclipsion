
using System;
using ProjectEclipsion.Core.Gameplay.StatusEffects;

namespace ProjectEclipsion.Core.Gameplay.Player;

public sealed class Player
{
    public Player(PlayerStats stats, int x = 0, int y = 0)
    {
        Stats = stats ?? throw new ArgumentNullException(nameof(stats));
        X = x;
        Y = y;
        SkillPoint = 3;
        StatusEffects = new StatusEffectList();
    }

    public PlayerStats Stats { get; }

    public int X { get; private set; }

    public int Y { get; private set; }

    public bool IsDead => Stats.IsDead;

    public int SkillPoint { get; private set; }

    public StatusEffectList StatusEffects { get; }

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
        // amountの内容が0以下の場合例をスローする。
        if (amount < 0)
        {
            // ArgumentOutOfRangeException
            // https://learn.microsoft.com/ja-jp/dotnet/api/system.argumentoutofrangeexception?view=net-10.0
            // 引数の値が、呼び出されたメソッドで定義されている値の許容範囲外にある場合にスローされる例外
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

    public void ApplyStatusEffect(StatusEffect effect)
    {
        StatusEffects.Apply(effect);
    }

    public void UpdateStatusEffects()
    {
        StatusEffects.Update(TakeDamage);
    }

    public bool TrySpendSkillPoints(int cost)
    {
        if (cost < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cost), "スキルポイント消費量は0以上である必要があります。");
        }

        if (SkillPoint < cost)
        {
            return false;
        }

        SkillPoint -= cost;
        return true;
    }
}
