using System.Collections.Generic;

namespace ProjectEclipsion.Core.Gameplay.Skills;

public static class TechTree
{
    public static SkillTree Create()
    {
        return new SkillTree(
            SkillTreeType.Tech,
            new List<SkillTreeNode>
            {
                new SkillTreeNode("tech_drone", "Drone", "ドローン制御を強化する。", cost: 1, SkillTreeType.Tech),
                new SkillTreeNode("tech_shield", "Shield", "シールド制御を強化する。", cost: 1, SkillTreeType.Tech),
                new SkillTreeNode("tech_hack", "Hack", "ハック能力を強化する。", cost: 1, SkillTreeType.Tech),
            });
    }
}
