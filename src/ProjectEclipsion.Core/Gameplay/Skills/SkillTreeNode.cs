using System;

namespace ProjectEclipsion.Core.Gameplay.Skills;

public sealed class SkillTreeNode
{
    public SkillTreeNode(string id, string name, string description, int cost, SkillTreeType treeType)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("スキルIDは必須です。", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("スキル名は必須です。", nameof(name));
        }

        if (cost < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cost), "コストは0以上である必要があります。");
        }

        Id = id;
        Name = name;
        Description = description;
        Cost = cost;
        TreeType = treeType;
    }

    public string Id { get; }

    public string Name { get; }

    public string Description { get; }

    public int Cost { get; }

    public bool IsUnlocked { get; private set; }

    public SkillTreeType TreeType { get; }

    public bool Unlock()
    {
        if (IsUnlocked)
        {
            return false;
        }

        IsUnlocked = true;
        return true;
    }
}
