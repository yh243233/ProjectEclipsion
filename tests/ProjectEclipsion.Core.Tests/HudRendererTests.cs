using ProjectEclipsion.App.Rendering;
using ProjectEclipsion.Core;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class HudRendererTests
{
    [Fact]
    public void CalculateRatio_HPとMaxHPからバー表示用の割合を計算できる()
    {
        Assert.Equal(0.8, HudRenderer.CalculateRatio(current: 80, max: 100));
    }

    [Fact]
    public void CalculateRatio_ShieldとMaxShieldからバー表示用の割合を計算できる()
    {
        Assert.Equal(0.5, HudRenderer.CalculateRatio(current: 25, max: 50));
    }

    [Fact]
    public void CalculateRatio_EnergyとMaxEnergyからバー表示用の割合を計算できる()
    {
        Assert.Equal(1.0, HudRenderer.CalculateRatio(current: 100, max: 100));
    }

    [Fact]
    public void FormatBar_HPが0の場合でもバー表示が破綻しない()
    {
        var bar = HudRenderer.FormatBar("HP", current: 0, max: 100);

        Assert.Contains("[----------]", bar);
        Assert.Contains("0/100", bar);
    }

    [Fact]
    public void FormatBar_HPがMaxHPを超えない前提で表示できる()
    {
        var bar = HudRenderer.FormatBar("HP", current: 100, max: 100);

        Assert.Contains("[##########]", bar);
        Assert.Contains("100/100", bar);
    }

    [Fact]
    public void FormatBar_最大値超過時でもバー表示が破綻しない()
    {
        var bar = HudRenderer.FormatBar("HP", current: 150, max: 100);

        Assert.Contains("[##########]", bar);
        Assert.Contains("150/100", bar);
    }

    [Fact]
    public void FormatCooldown_Cooldownを表示用に参照できる()
    {
        Assert.Equal("Ready", HudRenderer.FormatCooldown(0));
        Assert.Equal("3", HudRenderer.FormatCooldown(3));
    }

    [Fact]
    public void GameState_現在武器情報を参照できる()
    {
        var gameState = new GameState();

        Assert.Equal("Starter Assault", gameState.CurrentWeapon.Name);
        Assert.Equal(10, gameState.CurrentWeapon.Stats.Damage);
        Assert.Equal(ProjectEclipsion.Core.Gameplay.Weapons.BulletType.Normal, gameState.CurrentWeapon.BulletType);
    }
}
