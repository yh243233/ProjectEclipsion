using ProjectEclipsion.Core.Gameplay.Player;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class PlayerTests
{
    [Fact]
    public void PlayerStats_作成時にHPとShieldを最大値で初期化する()
    {
        var stats = new PlayerStats(maxHealth: 100, maxShield: 25, moveSpeed: 2);

        Assert.Equal(100, stats.MaxHealth);
        Assert.Equal(100, stats.Health);
        Assert.Equal(25, stats.MaxShield);
        Assert.Equal(25, stats.Shield);
        Assert.Equal(2, stats.MoveSpeed);
    }

    [Fact]
    public void Player_作成時にStatsと座標を保持する()
    {
        var stats = new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1);

        var player = new Player(stats, x: 5, y: 7);

        Assert.Same(stats, player.Stats);
        Assert.Equal(5, player.X);
        Assert.Equal(7, player.Y);
    }

    [Fact]
    public void Move_移動速度を反映して座標を更新する()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 3), x: 10, y: 20);

        player.Move(directionX: 1, directionY: -1);

        Assert.Equal(13, player.X);
        Assert.Equal(17, player.Y);
    }

    [Fact]
    public void MoveUp_Y座標が減る()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1), x: 0, y: 0);

        player.Move(directionX: 0, directionY: -1);

        Assert.Equal(0, player.X);
        Assert.Equal(-1, player.Y);
    }

    [Fact]
    public void MoveDown_Y座標が増える()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1), x: 0, y: 0);

        player.Move(directionX: 0, directionY: 1);

        Assert.Equal(0, player.X);
        Assert.Equal(1, player.Y);
    }

    [Fact]
    public void MoveLeft_X座標が減る()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1), x: 0, y: 0);

        player.Move(directionX: -1, directionY: 0);

        Assert.Equal(-1, player.X);
        Assert.Equal(0, player.Y);
    }

    [Fact]
    public void MoveRight_X座標が増える()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1), x: 0, y: 0);

        player.Move(directionX: 1, directionY: 0);

        Assert.Equal(1, player.X);
        Assert.Equal(0, player.Y);
    }

    [Fact]
    public void SetHealth_HPを範囲内に補正する()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1));

        player.SetHealth(150);
        Assert.Equal(100, player.Stats.Health);

        player.SetHealth(-10);
        Assert.Equal(0, player.Stats.Health);
    }

    [Fact]
    public void RestoreHealth_最大HPを超えない()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1));

        player.SetHealth(80);
        player.RestoreHealth(50);

        Assert.Equal(100, player.Stats.Health);
    }

    [Fact]
    public void SetShield_Shieldを範囲内に補正する()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1));

        player.SetShield(80);
        Assert.Equal(50, player.Stats.Shield);

        player.SetShield(-5);
        Assert.Equal(0, player.Stats.Shield);
    }

    [Fact]
    public void RestoreShield_最大Shieldを超えない()
    {
        var player = new Player(new PlayerStats(maxHealth: 100, maxShield: 50, moveSpeed: 1));

        player.SetShield(20);
        player.RestoreShield(40);

        Assert.Equal(50, player.Stats.Shield);
    }
}
