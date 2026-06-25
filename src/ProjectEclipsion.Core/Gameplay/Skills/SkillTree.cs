using System;
using System.Collections.Generic;
using System.Linq;
using PlayerEntity = ProjectEclipsion.Core.Gameplay.Player.Player;

namespace ProjectEclipsion.Core.Gameplay.Skills;

public sealed class SkillTree
{
    private readonly List<SkillTreeNode> nodes;

    public SkillTree(SkillTreeType type, IEnumerable<SkillTreeNode> nodes)
    {
        Type = type;
        this.nodes = nodes?.ToList() ?? throw new ArgumentNullException(nameof(nodes));
    }

    public SkillTreeType Type { get; }

    public IReadOnlyList<SkillTreeNode> Nodes => nodes;

    public IReadOnlyList<SkillTreeNode> UnlockedNodes => nodes.Where(node => node.IsUnlocked).ToList();

    public bool UnlockFirstAvailable(PlayerEntity player)
    {
        ArgumentNullException.ThrowIfNull(player);

        var node = nodes.FirstOrDefault(current => !current.IsUnlocked);
        if (node is null)
        {
            return false;
        }

        if (!player.TrySpendSkillPoints(node.Cost))
        {
            return false;
        }

        return node.Unlock();
    }
}
