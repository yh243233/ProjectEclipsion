using System;

namespace ProjectEclipsion.Core.Gameplay.Player;

public sealed class PlayerStats

{
    // ここがプレイヤーステータスのクラス内部。
    // PlayerStatsの様なクラス名とメソッド名が同じな場合、コンストラクタ扱いされてクラス宣言時に自動的に呼び出される。
    public PlayerStats(int maxHealth, int maxShield, int moveSpeed)
    {
        if (maxHealth <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxHealth), "最大HPは1以上である必要があります。");
        }

        if (maxShield < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxShield), "最大Shieldは0以上である必要があります。");
        }

        if (moveSpeed <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(moveSpeed), "移動速度は1以上である必要があります。");
        }

        // 基本的にプレイヤーステータスはここで初期化される。
        // 最大値とデフォルトの値を同じにしている。
        MaxHealth = maxHealth;
        Health = maxHealth;
        MaxShield = maxShield;
        Shield = maxShield;
        MoveSpeed = moveSpeed;
    }

    // 各種ゲッターメソッドとセッターメソッドを宣言している。
    public int MaxHealth { get; }

    public int Health { get; private set; }

    public int MaxShield { get; }

    public int Shield { get; private set; }

    public int MoveSpeed { get; }

    // 次の段階で使用する為正解かはわからないが恐らくステータスの更新はこの辺りで行う事になる。
    public void SetHealth(int value)
    {
        Health = Clamp(value, 0, MaxHealth);
    }

    public void SetShield(int value)
    {
        Shield = Clamp(value, 0, MaxShield);
    }

    public void RestoreHealth(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "回復量は0以上である必要があります。");
        }

        SetHealth(Health + amount);
    }

    public void RestoreShield(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Shield回復量は0以上である必要があります。");
        }

        SetShield(Shield + amount);
    }

    // 今回Clampは手動で準備している。
    // 通常組み込み関数のものを使うパターンもある。
    // 参考　https://aetheria.jp/19130/
    private static int Clamp(int value, int min, int max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }
}
