namespace ProjectEclipsion.Core.Gameplay.Items;

public sealed class Equipment
{
    public Item? EquippedItem { get; private set; }

    public void Equip(Item item)
    {
        EquippedItem = item;
    }
}
