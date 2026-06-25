using System;

namespace ProjectEclipsion.Core.Gameplay.StatusEffects;

public sealed class StatusEffect
{
    public StatusEffect(
        StatusEffectType type,
        int duration,
        int effectValue,
        double moveSpeedMultiplier = 1.0,
        bool preventsAction = false,
        double damageTakenMultiplier = 1.0,
        bool preventsSkillUse = false)
    {
        if (duration < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(duration), "効果時間は0以上である必要があります。");
        }

        if (effectValue < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(effectValue), "効果値は0以上である必要があります。");
        }

        Type = type;
        Duration = duration;
        EffectValue = effectValue;
        MoveSpeedMultiplier = moveSpeedMultiplier;
        PreventsAction = preventsAction;
        DamageTakenMultiplier = damageTakenMultiplier;
        PreventsSkillUse = preventsSkillUse;
    }

    public StatusEffectType Type { get; }

    public int Duration { get; private set; }

    public int EffectValue { get; }

    public double MoveSpeedMultiplier { get; }

    public bool PreventsAction { get; }

    public double DamageTakenMultiplier { get; }

    public bool PreventsSkillUse { get; }

    public void TickDuration()
    {
        Duration--;
    }
}
