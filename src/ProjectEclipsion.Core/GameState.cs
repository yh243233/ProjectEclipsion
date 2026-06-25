using ProjectEclipsion.Core.Gameplay.Player;
using System.Collections.Generic;
using ProjectEclipsion.Core.Gameplay.Enemies;
using ProjectEclipsion.Core.Gameplay.Weapons;

namespace ProjectEclipsion.Core;

public sealed class GameState
{
    public GameState()
    {
        // ここでゲームクラスとタイトルを宣言してステータスの数字も入れている。
        // ここのPlayerStatsで宣言された値がデフォルト値になる。
        // TODO：ここでインスタンス宣言をPlayerStatsでやっているがその後PlayerStatsの内部メソッドが何故呼び出されているのかがわからない。
        // クラス内部ではコンストラクタの類も宣言されていない様に見える。
        // 解決済み：下の公式ページで読んだところC#のコンストラクタはクラスと同じ名前で宣言されるもので、クラスのインスタンスが生成される際に自動的に呼び出されるものらしい。
        Title = "Project Eclipsion";
        Player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1));
        CurrentWeapon = new Weapon(WeaponCategory.Assault, new WeaponStats(damage: 10, bulletSpeed: 1));
        Bullets = new List<Bullet>();
        Enemies = new List<Enemy>
        {
            new Enemy(x: 30, y: 10, maxHealth: 30, aiLevel: EnemyAiLevel.Basic),
        };
    }

    public string Title { get; }
    // TODO：プレイヤーの型宣言をそのままクラスのプロパティを使って定義する事は意味があるのかを調べる。
    // 解決済み：「このデータはPlayer型として扱う」という設計意図を明確にして「PlayerプロパティにはPlayer型しか入れない」というルールを作っている状態で
    // あるためダブルチェックとはまた違うものになる。
    public Player Player { get; }

    public Weapon CurrentWeapon { get; }

    public List<Bullet> Bullets { get; }

    public List<Enemy> Enemies { get; }

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

    public void Update()
    {
        foreach (var bullet in Bullets)
        {
            bullet.Update();
        }

        foreach (var enemy in Enemies)
        {
            enemy.Update(Player.X, Player.Y);
        }
    }
}
