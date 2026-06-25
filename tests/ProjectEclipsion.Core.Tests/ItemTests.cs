using ProjectEclipsion.Core.Gameplay.Items;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class ItemTests
{
    [Fact]
    public void ItemRarity_必要なレアリティが存在する()
    {
        Assert.True(System.Enum.IsDefined(typeof(ItemRarity), ItemRarity.Common));
        Assert.True(System.Enum.IsDefined(typeof(ItemRarity), ItemRarity.Rare));
        Assert.True(System.Enum.IsDefined(typeof(ItemRarity), ItemRarity.Epic));
        Assert.True(System.Enum.IsDefined(typeof(ItemRarity), ItemRarity.Legendary));
        Assert.True(System.Enum.IsDefined(typeof(ItemRarity), ItemRarity.Exotic));
    }

    [Fact]
    public void Item_作成できる()
    {
        var item = new Item("Overclock Core", ItemRarity.Rare, "武器出力を高めるコア。", powerBonus: 5);

        Assert.Equal("Overclock Core", item.Name);
        Assert.Equal(ItemRarity.Rare, item.Rarity);
        Assert.Equal("武器出力を高めるコア。", item.Description);
        Assert.Equal(5, item.PowerBonus);
    }

    [Fact]
    public void Inventory_Itemを追加できる()
    {
        var inventory = new Inventory();
        var item = new Item("Overclock Core", ItemRarity.Rare, "武器出力を高めるコア。", powerBonus: 5);

        inventory.Add(item);

        Assert.Same(item, inventory.Items[0]);
    }

    [Fact]
    public void Inventory_Item数を取得できる()
    {
        var inventory = new Inventory();

        inventory.Add(new Item("Overclock Core", ItemRarity.Rare, "武器出力を高めるコア。", powerBonus: 5));

        Assert.Equal(1, inventory.Count);
    }

    [Fact]
    public void Equipment_Itemを装備できる()
    {
        var equipment = new Equipment();
        var item = new Item("Overclock Core", ItemRarity.Rare, "武器出力を高めるコア。", powerBonus: 5);

        equipment.Equip(item);

        Assert.Same(item, equipment.EquippedItem);
    }
}
