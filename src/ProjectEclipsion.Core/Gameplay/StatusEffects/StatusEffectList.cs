using System;
using System.Collections.Generic;

namespace ProjectEclipsion.Core.Gameplay.StatusEffects;

public sealed class StatusEffectList
{
    private readonly List<StatusEffect> effects = new();

    public IReadOnlyList<StatusEffect> Effects => effects;

    public int Count => effects.Count;

    public void Apply(StatusEffect effect)
    {
        ArgumentNullException.ThrowIfNull(effect);

        var existingIndex = effects.FindIndex(current => current.Type == effect.Type);
        if (existingIndex >= 0)
        {
            effects[existingIndex] = effect;
            return;
        }

        effects.Add(effect);
    }

    public bool Has(StatusEffectType type)
    {
        return effects.Exists(effect => effect.Type == type);
    }

    public StatusEffect? Get(StatusEffectType type)
    {
        return effects.Find(effect => effect.Type == type);
    }

    public void Update(Action<int> applyDamage)
    {
        ArgumentNullException.ThrowIfNull(applyDamage);

        foreach (var effect in effects)
        {
            if (effect.Type == StatusEffectType.Burn)
            {
                applyDamage(effect.EffectValue);
            }

            effect.TickDuration();
        }

        effects.RemoveAll(effect => effect.Duration <= 0);
    }
}
