using System.Collections.Generic;

namespace ProjectEclipsion.Core.Gameplay.Skills;

public static class SurvivalTree
{
    public static SkillTree Create()
    {
        return new SkillTree(
            SkillTreeType.Survival,
            new List<SkillTreeNode>
            {
                new SkillTreeNode("survival_hp", "HP", "耐久力を強化する。", cost: 1, SkillTreeType.Survival),
                new SkillTreeNode("survival_dash", "Dash", "回避行動を強化する。", cost: 1, SkillTreeType.Survival),
                new SkillTreeNode("survival_regen", "Regen", "回復能力を強化する。", cost: 1, SkillTreeType.Survival),
            });
    }
}
