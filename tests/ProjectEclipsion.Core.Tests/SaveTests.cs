using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectEclipsion.Core;
using ProjectEclipsion.Core.Save;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class SaveTests
{
    [Fact]
    public void SaveDataを作成できる()
    {
        var saveData = new SaveData
        {
            PlayerX = 3,
            PlayerY = 4,
            PlayerHp = 80,
            PlayerShield = 20,
            PlayerEnergy = 90,
            Score = 100,
            CurrentWeaponName = "Starter Assault",
            UnlockedWeaponNames = new List<string> { "Starter Assault" },
        };

        Assert.Equal(3, saveData.PlayerX);
        Assert.Equal(4, saveData.PlayerY);
        Assert.Equal(80, saveData.PlayerHp);
        Assert.Equal(20, saveData.PlayerShield);
        Assert.Equal(90, saveData.PlayerEnergy);
        Assert.Equal(100, saveData.Score);
        Assert.Equal("Starter Assault", saveData.CurrentWeaponName);
        Assert.Contains("Starter Assault", saveData.UnlockedWeaponNames);
    }

    [Fact]
    public void GameStateからSaveDataを作成できる()
    {
        var gameState = new GameState();
        gameState.MovePlayer(2, 1);
        gameState.DamagePlayer(10);
        gameState.SwitchWeapon(5);

        var saveData = gameState.ToSaveData();

        Assert.Equal(2, saveData.PlayerX);
        Assert.Equal(1, saveData.PlayerY);
        Assert.Equal(100, saveData.PlayerHp);
        Assert.Equal(40, saveData.PlayerShield);
        Assert.Equal(100, saveData.PlayerEnergy);
        Assert.Equal(0, saveData.Score);
        Assert.Equal("Impact Rocket", saveData.CurrentWeaponName);
        Assert.Contains("Starter Assault", saveData.UnlockedWeaponNames);
        Assert.Contains("Impact Rocket", saveData.UnlockedWeaponNames);
    }

    [Fact]
    public void SaveDataからGameStateを復元できる()
    {
        var gameState = new GameState();
        var saveData = CreateSaveData();

        gameState.ApplySaveData(saveData);

        Assert.Equal(7, gameState.Player.X);
        Assert.Equal(-2, gameState.Player.Y);
        Assert.Equal(65, gameState.Player.Stats.Health);
        Assert.Equal(15, gameState.Player.Stats.Shield);
        Assert.Equal(70, gameState.Player.Stats.Energy);
        Assert.Equal(300, gameState.Score);
        Assert.Equal("Impact Rocket", gameState.CurrentWeapon.Name);
        Assert.Equal(saveData.UnlockedWeaponNames.OrderBy(name => name), gameState.UnlockedWeaponNames.OrderBy(name => name));
    }

    [Fact]
    public void Player座標が保存復元される()
    {
        var gameState = new GameState();

        gameState.ApplySaveData(CreateSaveData(playerX: 9, playerY: 8));

        Assert.Equal(9, gameState.Player.X);
        Assert.Equal(8, gameState.Player.Y);
    }

    [Fact]
    public void PlayerHPが保存復元される()
    {
        var gameState = new GameState();

        gameState.ApplySaveData(CreateSaveData(playerHp: 42));

        Assert.Equal(42, gameState.Player.Stats.Health);
    }

    [Fact]
    public void PlayerShieldが保存復元される()
    {
        var gameState = new GameState();

        gameState.ApplySaveData(CreateSaveData(playerShield: 12));

        Assert.Equal(12, gameState.Player.Stats.Shield);
    }

    [Fact]
    public void PlayerEnergyが保存復元される()
    {
        var gameState = new GameState();

        gameState.ApplySaveData(CreateSaveData(playerEnergy: 33));

        Assert.Equal(33, gameState.Player.Stats.Energy);
    }

    [Fact]
    public void Scoreが保存復元される()
    {
        var gameState = new GameState();

        gameState.ApplySaveData(CreateSaveData(score: 500));

        Assert.Equal(500, gameState.Score);
    }

    [Fact]
    public void 現在武器名が保存復元される()
    {
        var gameState = new GameState();

        gameState.ApplySaveData(CreateSaveData(currentWeaponName: "Longshot Sniper"));

        Assert.Equal("Longshot Sniper", gameState.CurrentWeapon.Name);
    }

    [Fact]
    public void 解放済み武器一覧が保存復元される()
    {
        var gameState = new GameState();
        var unlockedWeaponNames = new List<string> { "Starter Assault", "Focus Beam" };

        gameState.ApplySaveData(CreateSaveData(unlockedWeaponNames: unlockedWeaponNames));

        Assert.Equal(unlockedWeaponNames.OrderBy(name => name), gameState.UnlockedWeaponNames.OrderBy(name => name));
    }

    [Fact]
    public void SaveJsonへ保存できる()
    {
        var savePath = CreateTempSavePath();
        try
        {
            var repository = new JsonSaveRepository(savePath);

            repository.Save(CreateSaveData());

            Assert.True(File.Exists(savePath));
        }
        finally
        {
            DeleteTempSaveDirectory(savePath);
        }
    }

    [Fact]
    public void SaveJsonからロードできる()
    {
        var savePath = CreateTempSavePath();
        try
        {
            var repository = new JsonSaveRepository(savePath);
            repository.Save(CreateSaveData(playerX: 11, score: 900));

            var result = repository.TryLoad(out var loadedSaveData);

            Assert.True(result);
            Assert.NotNull(loadedSaveData);
            Assert.Equal(11, loadedSaveData.PlayerX);
            Assert.Equal(900, loadedSaveData.Score);
        }
        finally
        {
            DeleteTempSaveDirectory(savePath);
        }
    }

    [Fact]
    public void SaveJsonが存在しない場合に安全に失敗できる()
    {
        var savePath = CreateTempSavePath();
        var repository = new JsonSaveRepository(savePath);

        var result = repository.TryLoad(out var loadedSaveData);

        Assert.False(result);
        Assert.Null(loadedSaveData);
    }

    private static SaveData CreateSaveData(
        int playerX = 7,
        int playerY = -2,
        int playerHp = 65,
        int playerShield = 15,
        int playerEnergy = 70,
        int score = 300,
        string currentWeaponName = "Impact Rocket",
        List<string>? unlockedWeaponNames = null)
    {
        return new SaveData
        {
            PlayerX = playerX,
            PlayerY = playerY,
            PlayerHp = playerHp,
            PlayerShield = playerShield,
            PlayerEnergy = playerEnergy,
            Score = score,
            CurrentWeaponName = currentWeaponName,
            UnlockedWeaponNames = unlockedWeaponNames ?? new List<string> { "Starter Assault", "Impact Rocket" },
        };
    }

    private static string CreateTempSavePath()
    {
        return Path.Combine(Path.GetTempPath(), "ProjectEclipsionTests", Guid.NewGuid().ToString("N"), "save.json");
    }

    private static void DeleteTempSaveDirectory(string savePath)
    {
        var directory = Path.GetDirectoryName(savePath);
        if (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
