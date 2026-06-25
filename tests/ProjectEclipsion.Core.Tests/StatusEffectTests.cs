using ProjectEclipsion.Core.Gameplay.StatusEffects;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class StatusEffectTests
{
    [Fact]
    public void StatusEffectType_必要な状態異常が存在する()
    {
        Assert.True(System.Enum.IsDefined(typeof(StatusEffectType), StatusEffectType.Burn));
        Assert.True(System.Enum.IsDefined(typeof(StatusEffectType), StatusEffectType.Freeze));
        Assert.True(System.Enum.IsDefined(typeof(StatusEffectType), StatusEffectType.Shock));
        Assert.True(System.Enum.IsDefined(typeof(StatusEffectType), StatusEffectType.Corrosion));
        Assert.True(System.Enum.IsDefined(typeof(StatusEffectType), StatusEffectType.Virus));
    }

    [Fact]
    public void StatusEffect_作成できる()
    {
        var effect = new StatusEffect(StatusEffectType.Burn, duration: 3, effectValue: 5);

        Assert.Equal(StatusEffectType.Burn, effect.Type);
        Assert.Equal(3, effect.Duration);
        Assert.Equal(5, effect.EffectValue);
    }

    [Fact]
    public void StatusEffectList_UpdateでDurationが減る()
    {
        var effects = new StatusEffectList();
        effects.Apply(new StatusEffect(StatusEffectType.Freeze, duration: 3, effectValue: 0));

        effects.Update(_ => { });

        Assert.Equal(2, effects.Get(StatusEffectType.Freeze)?.Duration);
    }

    [Fact]
    public void StatusEffectList_Durationが0以下の状態異常を削除する()
    {
        var effects = new StatusEffectList();
        effects.Apply(new StatusEffect(StatusEffectType.Freeze, duration: 1, effectValue: 0));

        effects.Update(_ => { });

        Assert.False(effects.Has(StatusEffectType.Freeze));
    }

    [Fact]
    public void StatusEffectList_同じ状態異常を再付与した場合にDurationを更新する()
    {
        var effects = new StatusEffectList();
        effects.Apply(new StatusEffect(StatusEffectType.Burn, duration: 1, effectValue: 5));

        effects.Apply(new StatusEffect(StatusEffectType.Burn, duration: 5, effectValue: 5));

        Assert.Single(effects.Effects);
        Assert.Equal(5, effects.Get(StatusEffectType.Burn)?.Duration);
    }

    [Fact]
    public void Freeze_移動速度倍率を持つ()
    {
        var effect = new StatusEffect(StatusEffectType.Freeze, duration: 5, effectValue: 0, moveSpeedMultiplier: 0.5);

        Assert.Equal(0.5, effect.MoveSpeedMultiplier);
    }

    [Fact]
    public void Shock_行動不可フラグを持つ()
    {
        var effect = new StatusEffect(StatusEffectType.Shock, duration: 2, effectValue: 0, preventsAction: true);

        Assert.True(effect.PreventsAction);
    }

    [Fact]
    public void Corrosion_被ダメージ倍率を持つ()
    {
        var effect = new StatusEffect(StatusEffectType.Corrosion, duration: 4, effectValue: 0, damageTakenMultiplier: 1.5);

        Assert.Equal(1.5, effect.DamageTakenMultiplier);
    }

    [Fact]
    public void Virus_スキル使用不可フラグを持つ()
    {
        var effect = new StatusEffect(StatusEffectType.Virus, duration: 4, effectValue: 0, preventsSkillUse: true);

        Assert.True(effect.PreventsSkillUse);
    }
}
