using System.Collections.Generic;

namespace ProjectEclipsion.Core.Save;

public sealed class SaveData
{
    public int PlayerX { get; set; }

    public int PlayerY { get; set; }

    public int PlayerHp { get; set; }

    public int PlayerShield { get; set; }

    public int PlayerEnergy { get; set; }

    public int Score { get; set; }

    public string CurrentWeaponName { get; set; } = string.Empty;

    public List<string> UnlockedWeaponNames { get; set; } = new();
}
