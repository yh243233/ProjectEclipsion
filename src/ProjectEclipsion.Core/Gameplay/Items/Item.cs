using System;

namespace ProjectEclipsion.Core.Gameplay.Items;

public sealed class Item
{
    public Item(string name, ItemRarity rarity, string description, int powerBonus)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("アイテム名は空にできません。", nameof(name));
        }

        Name = name;
        Rarity = rarity;
        Description = description ?? string.Empty;
        PowerBonus = powerBonus;
    }

    public string Name { get; }

    public ItemRarity Rarity { get; }

    public string Description { get; }

    public int PowerBonus { get; }
}
