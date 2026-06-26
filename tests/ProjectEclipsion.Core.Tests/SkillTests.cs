using ProjectEclipsion.Core.Gameplay.Player;
using ProjectEclipsion.Core.Gameplay.Skills;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class SkillTests
{
    [Fact]
    public void SkillTreeNode_作成できる()
    {
        var node = new SkillTreeNode("combat_critical", "Critical", "クリティカル性能を強化する。", cost: 1, SkillTreeType.Combat);

        Assert.Equal("combat_critical", node.Id);
        Assert.Equal("Critical", node.Name);
        Assert.Equal("クリティカル性能を強化する。", node.Description);
        Assert.Equal(1, node.Cost);
        Assert.False(node.IsUnlocked);
        Assert.Equal(SkillTreeType.Combat, node.TreeType);
        Assert.Equal(0, node.Cooldown);
    }

    [Fact]
    public void SkillTreeNode_Cooldownを表示用に参照できる()
    {
        var node = new SkillTreeNode("tech_shield", "Shield", "シールド制御を強化する。", cost: 1, SkillTreeType.Tech, cooldown: 3);

        Assert.Equal(3, node.Cooldown);
    }

    [Fact]
    public void SkillTreeType_必要な種類が存在する()
    {
        Assert.True(System.Enum.IsDefined(typeof(SkillTreeType), SkillTreeType.Combat));
        Assert.True(System.Enum.IsDefined(typeof(SkillTreeType), SkillTreeType.Tech));
        Assert.True(System.Enum.IsDefined(typeof(SkillTreeType), SkillTreeType.Survival));
    }

    [Fact]
    public void CombatTree_必要なスキルが存在する()
    {
        var tree = CombatTree.Create();

        Assert.Contains(tree.Nodes, node => node.Name == "Critical");
        Assert.Contains(tree.Nodes, node => node.Name == "Reload");
        Assert.Contains(tree.Nodes, node => node.Name == "Accuracy");
    }

    [Fact]
    public void TechTree_必要なスキルが存在する()
    {
        var tree = TechTree.Create();

        Assert.Contains(tree.Nodes, node => node.Name == "Drone");
        Assert.Contains(tree.Nodes, node => node.Name == "Shield");
        Assert.Contains(tree.Nodes, node => node.Name == "Hack");
    }

    [Fact]
    public void SurvivalTree_必要なスキルが存在する()
    {
        var tree = SurvivalTree.Create();

        Assert.Contains(tree.Nodes, node => node.Name == "HP");
        Assert.Contains(tree.Nodes, node => node.Name == "Dash");
        Assert.Contains(tree.Nodes, node => node.Name == "Regen");
    }

    [Fact]
    public void UnlockFirstAvailable_SkillPointを消費してスキルを解放できる()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1));
        var tree = CombatTree.Create();

        var result = tree.UnlockFirstAvailable(player);

        Assert.True(result);
        Assert.Equal(2, player.SkillPoint);
        Assert.True(tree.Nodes[0].IsUnlocked);
    }

    [Fact]
    public void UnlockFirstAvailable_SkillPoint不足時は解放できない()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1));
        player.TrySpendSkillPoints(3);
        var tree = CombatTree.Create();

        var result = tree.UnlockFirstAvailable(player);

        Assert.False(result);
        Assert.False(tree.Nodes[0].IsUnlocked);
    }

    [Fact]
    public void SkillTreeNode_解放済みスキルは再解放できない()
    {
        var node = new SkillTreeNode("combat_critical", "Critical", "クリティカル性能を強化する。", cost: 1, SkillTreeType.Combat);

        Assert.True(node.Unlock());
        Assert.False(node.Unlock());
    }
}
