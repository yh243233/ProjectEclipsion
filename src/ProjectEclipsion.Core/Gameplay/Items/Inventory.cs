using System.Collections.Generic;

namespace ProjectEclipsion.Core.Gameplay.Items;

public sealed class Inventory
{
    private readonly List<Item> items = new();

    public IReadOnlyList<Item> Items => items;

    public int Count => items.Count;

    public void Add(Item item)
    {
        items.Add(item);
    }

    public Item? GetFirstItem()
    {
        return items.Count == 0 ? null : items[0];
    }
}
