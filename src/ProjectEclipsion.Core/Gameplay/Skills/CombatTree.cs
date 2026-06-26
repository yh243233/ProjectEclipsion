using System.Collections.Generic;

namespace ProjectEclipsion.Core.Gameplay.Skills;

public static class CombatTree
{
    public static SkillTree Create()
    {
        return new SkillTree(
            SkillTreeType.Combat,
            new List<SkillTreeNode>
            {
                new SkillTreeNode("combat_critical", "Critical", "クリティカル性能を強化する。", cost: 1, SkillTreeType.Combat, cooldown: 0),
                new SkillTreeNode("combat_reload", "Reload", "リロード性能を強化する。", cost: 1, SkillTreeType.Combat, cooldown: 2),
                new SkillTreeNode("combat_accuracy", "Accuracy", "命中精度を強化する。", cost: 1, SkillTreeType.Combat, cooldown: 1),
            });
    }
}
