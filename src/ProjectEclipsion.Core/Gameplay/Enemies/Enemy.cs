using System;

namespace ProjectEclipsion.Core.Gameplay.Enemies;

public sealed class Enemy
{
    public Enemy(int x, int y, int maxHealth, EnemyAiLevel aiLevel)
    {
        if (maxHealth < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxHealth), "最大HPは0以上である必要があります。");
        }

        X = x;
        Y = y;
        MaxHealth = maxHealth;
        Health = maxHealth;
        AiLevel = aiLevel;
        AiState = EnemyAiState.Idle;
    }

    public int X { get; private set; }

    public int Y { get; private set; }

    public int MaxHealth { get; }

    public int Health { get; private set; }

    public EnemyAiLevel AiLevel { get; }

    public EnemyAiState AiState { get; private set; }

    public bool IsDead => Health == 0;

    public void Update(int playerX, int playerY)
    {
        AiState = EnemyAiState.Combat;

        if (X < playerX)
        {
            X++;
        }
        else if (X > playerX)
        {
            X--;
        }

        if (Y < playerY)
        {
            Y++;
        }
        else if (Y > playerY)
        {
            Y--;
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "ダメージ量は0以上である必要があります。");
        }

        Health = Math.Max(0, Health - amount);
    }
}
