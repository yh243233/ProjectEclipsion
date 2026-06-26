using ProjectEclipsion.Core.Gameplay.Player;
using System.Collections.Generic;
using ProjectEclipsion.Core.Gameplay.Enemies;
using ProjectEclipsion.Core.Gameplay.Items;
using ProjectEclipsion.Core.Gameplay.Skills;
using ProjectEclipsion.Core.Gameplay.StatusEffects;
using ProjectEclipsion.Core.Gameplay.Weapons;
using ProjectEclipsion.Core.Gameplay.World.Generation;
using ProjectEclipsion.Core.Gameplay.World.Maps;
using ProjectEclipsion.Core.Gameplay.World.Rooms;

namespace ProjectEclipsion.Core;

public sealed class GameState
{
    private const int ScorePerEnemyDefeat = 100;

    public GameState()
    {
        // ここでゲームクラスとタイトルを宣言してステータスの数字も入れている。
        // ここのPlayerStatsで宣言された値がデフォルト値になる。
        // TODO：ここでインスタンス宣言をPlayerStatsでやっているがその後PlayerStatsの内部メソッドが何故呼び出されているのかがわからない。
        // クラス内部ではコンストラクタの類も宣言されていない様に見える。
        // 解決済み：下の公式ページで読んだところC#のコンストラクタはクラスと同じ名前で宣言されるもので、クラスのインスタンスが生成される際に自動的に呼び出されるものらしい。
        Title = "Project Eclipsion";
        Score = 0;
        Player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1));
        Weapons = CreateInitialWeapons();
        CurrentWeapon = Weapons[0];
        Bullets = new List<Bullet>();
        DroppedItems = new List<Item>();
        Inventory = new Inventory();
        Equipment = new Equipment();
        CombatSkillTree = CombatTree.Create();
        TechSkillTree = TechTree.Create();
        SurvivalSkillTree = SurvivalTree.Create();
        GameMap = new BspMapGenerator().Generate();
        Enemies = new List<Enemy>
        {
            new Enemy(x: 30, y: 10, maxHealth: 30, aiLevel: EnemyAiLevel.Basic),
        };
    }

    public string Title { get; }

    public int Score { get; private set; }

    // TODO：プレイヤーの型宣言をそのままクラスのプロパティを使って定義する事は意味があるのかを調べる。
    // 解決済み：「このデータはPlayer型として扱う」という設計意図を明確にして「PlayerプロパティにはPlayer型しか入れない」というルールを作っている状態で
    // あるためダブルチェックとはまた違うものになる。
    public Player Player { get; }

    public List<Weapon> Weapons { get; }

    public Weapon CurrentWeapon { get; private set; }

    public List<Bullet> Bullets { get; }

    public List<Enemy> Enemies { get; }

    public List<Item> DroppedItems { get; }

    public Inventory Inventory { get; }

    public Equipment Equipment { get; }

    public SkillTree CombatSkillTree { get; }

    public SkillTree TechSkillTree { get; }

    public SkillTree SurvivalSkillTree { get; }

    public GameMap GameMap { get; }

    public void MovePlayer(int directionX, int directionY)
    {
        Player.Move(directionX, directionY);
    }

    // program.cs内部でPlayerクラスのMoveメソッドを呼び出すためのラッパーメソッド。
    public void DamagePlayer(int amount)
    {
        Player.TakeDamage(amount);
    }

    public void FireCurrentWeapon()
    {
        var bullet = CurrentWeapon.Fire(Player.X, Player.Y, directionX: 1, directionY: 0);
        Bullets.Add(bullet);
    }

    public void SwitchWeapon(int weaponNumber)
    {
        if (weaponNumber < 1 || weaponNumber > Weapons.Count)
        {
            return;
        }

        CurrentWeapon = Weapons[weaponNumber - 1];
    }

    public void Update()
    {
        foreach (var bullet in Bullets)
        {
            UpdateHomingBulletDirection(bullet);
            bullet.Update();
        }

        foreach (var enemy in Enemies)
        {
            enemy.UpdateStatusEffects();
            if (enemy.IsDead)
            {
                continue;
            }

            enemy.Update(Player.X, Player.Y);
        }

        Player.UpdateStatusEffects();
        HandleBulletEnemyCollisions();
        Bullets.RemoveAll(bullet => !bullet.IsActive);
        Enemies.RemoveAll(enemy => enemy.IsDead);
    }

    public void ApplyStatusEffectToFirstEnemy(StatusEffectType type)
    {
        if (Enemies.Count == 0)
        {
            return;
        }

        Enemies[0].ApplyStatusEffect(CreateDefaultStatusEffect(type));
    }

    public bool UnlockFirstSkill(SkillTreeType treeType)
    {
        return GetSkillTree(treeType).UnlockFirstAvailable(Player);
    }

    public bool MoveRoom(RoomDirection direction)
    {
        return GameMap.TryMove(direction);
    }

    public void ToggleMiniMap()
    {
        GameMap.ToggleMiniMap();
    }

    private void HandleBulletEnemyCollisions()
    {
        foreach (var bullet in Bullets)
        {
            if (!bullet.IsActive)
            {
                continue;
            }

            foreach (var enemy in Enemies)
            {
                if (enemy.IsDead)
                {
                    continue;
                }

                if (bullet.X != enemy.X || bullet.Y != enemy.Y)
                {
                    continue;
                }

                var wasDead = enemy.IsDead;
                enemy.TakeDamage(bullet.Damage);
                if (!wasDead && enemy.IsDead)
                {
                    Score += ScorePerEnemyDefeat;
                    DropItemForEnemyDefeat();
                }

                if (!bullet.TryConsumePierce())
                {
                    bullet.Deactivate();
                }

                break;
            }
        }
    }

    public void PickUpFirstDroppedItem()
    {
        if (DroppedItems.Count == 0)
        {
            return;
        }

        var item = DroppedItems[0];
        DroppedItems.RemoveAt(0);
        Inventory.Add(item);
    }

    public void EquipFirstInventoryItem()
    {
        var item = Inventory.GetFirstItem();
        if (item is null)
        {
            return;
        }

        Equipment.Equip(item);
    }

    private void DropItemForEnemyDefeat()
    {
        DroppedItems.Add(CreateDefaultDropItem());
    }

    private static Item CreateDefaultDropItem()
    {
        return new Item(
            "Overclock Core",
            ItemRarity.Rare,
            "武器出力を高めるコア。",
            powerBonus: 5);
    }

    private static StatusEffect CreateDefaultStatusEffect(StatusEffectType type)
    {
        return type switch
        {
            StatusEffectType.Burn => new StatusEffect(type, duration: 3, effectValue: 5),
            StatusEffectType.Freeze => new StatusEffect(type, duration: 5, effectValue: 0, moveSpeedMultiplier: 0.5),
            StatusEffectType.Shock => new StatusEffect(type, duration: 2, effectValue: 0, preventsAction: true),
            StatusEffectType.Corrosion => new StatusEffect(type, duration: 4, effectValue: 0, damageTakenMultiplier: 1.5),
            StatusEffectType.Virus => new StatusEffect(type, duration: 4, effectValue: 0, preventsSkillUse: true),
            _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, "未対応の状態異常です。"),
        };
    }

    private SkillTree GetSkillTree(SkillTreeType treeType)
    {
        return treeType switch
        {
            SkillTreeType.Combat => CombatSkillTree,
            SkillTreeType.Tech => TechSkillTree,
            SkillTreeType.Survival => SurvivalSkillTree,
            _ => throw new System.ArgumentOutOfRangeException(nameof(treeType), treeType, "未対応のスキルツリーです。"),
        };
    }

    private void UpdateHomingBulletDirection(Bullet bullet)
    {
        if (!bullet.CanHome || Enemies.Count == 0)
        {
            return;
        }

        var nearestEnemy = FindNearestEnemy(bullet);
        if (nearestEnemy is null)
        {
            return;
        }

        bullet.SetHomingTarget(nearestEnemy.X, nearestEnemy.Y);
        bullet.SetDirection(GetStepDirection(bullet.X, nearestEnemy.X), GetStepDirection(bullet.Y, nearestEnemy.Y));
    }

    private Enemy? FindNearestEnemy(Bullet bullet)
    {
        Enemy? nearestEnemy = null;
        var nearestDistance = int.MaxValue;

        foreach (var enemy in Enemies)
        {
            if (enemy.IsDead)
            {
                continue;
            }

            var distance = GetDistance(bullet.X, bullet.Y, enemy.X, enemy.Y);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private static int GetDistance(int x1, int y1, int x2, int y2)
    {
        return System.Math.Abs(x1 - x2) + System.Math.Abs(y1 - y2);
    }

    private static int GetStepDirection(int current, int target)
    {
        if (current < target)
        {
            return 1;
        }

        if (current > target)
        {
            return -1;
        }

        return 0;
    }

    private static List<Weapon> CreateInitialWeapons()
    {
        return new List<Weapon>
        {
            new Weapon("Starter Assault", WeaponCategory.Assault, new WeaponStats(damage: 10, bulletSpeed: 1, fireRate: 3.0, reloadTime: 1.5)),
            new Weapon("Scatter Shotgun", WeaponCategory.Shotgun, new WeaponStats(damage: 8, bulletSpeed: 1, fireRate: 1.5, reloadTime: 2.0)),
            new Weapon("Longshot Sniper", WeaponCategory.Sniper, new WeaponStats(damage: 25, bulletSpeed: 3, fireRate: 0.8, reloadTime: 2.5)),
            new Weapon("Focus Beam", WeaponCategory.Beam, new WeaponStats(damage: 6, bulletSpeed: 2, fireRate: 4.0, reloadTime: 1.2)),
            new Weapon("Impact Rocket", WeaponCategory.Rocket, new WeaponStats(damage: 30, bulletSpeed: 1, fireRate: 0.6, reloadTime: 3.0)),
            new Weapon("Support Drone", WeaponCategory.Drone, new WeaponStats(damage: 5, bulletSpeed: 2, fireRate: 2.5, reloadTime: 1.8)),
        };
    }
}
