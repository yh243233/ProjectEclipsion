using ProjectEclipsion.Core.Gameplay.World.Biomes;
using ProjectEclipsion.Core.Gameplay.World.Maps;
using ProjectEclipsion.Core.Gameplay.World.Rooms;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class WorldTests
{
    [Fact]
    public void BiomeType_必要なバイオームが存在する()
    {
        Assert.True(System.Enum.IsDefined(typeof(BiomeType), BiomeType.UndergroundFacility));
        Assert.True(System.Enum.IsDefined(typeof(BiomeType), BiomeType.AbandonedFactory));
        Assert.True(System.Enum.IsDefined(typeof(BiomeType), BiomeType.Snowfield));
        Assert.True(System.Enum.IsDefined(typeof(BiomeType), BiomeType.Laboratory));
        Assert.True(System.Enum.IsDefined(typeof(BiomeType), BiomeType.Fortress));
        Assert.True(System.Enum.IsDefined(typeof(BiomeType), BiomeType.Otherworld));
    }

    [Fact]
    public void Room_作成できる()
    {
        var room = new Room("central", "Central Hub", x: 0, y: 0, BiomeType.UndergroundFacility);

        Assert.Equal("central", room.Id);
        Assert.Equal("Central Hub", room.Name);
        Assert.Equal(0, room.X);
        Assert.Equal(0, room.Y);
        Assert.Equal(BiomeType.UndergroundFacility, room.BiomeType);
    }

    [Fact]
    public void Room_初期IsVisitedはfalseである()
    {
        var room = new Room("central", "Central Hub", x: 0, y: 0, BiomeType.UndergroundFacility);

        Assert.False(room.IsVisited);
    }

    [Fact]
    public void GameMap_作成できる()
    {
        var gameMap = GameMap.CreatePhase2Default();

        Assert.NotNull(gameMap);
        Assert.NotEmpty(gameMap.Rooms);
    }

    [Fact]
    public void GameMap_初期Roomを持つ()
    {
        var gameMap = GameMap.CreatePhase2Default();

        Assert.Equal("Central Hub", gameMap.CurrentRoom.Name);
        Assert.True(gameMap.CurrentRoom.IsVisited);
    }

    [Fact]
    public void TryMove_接続済み方向へRoom移動できる()
    {
        var gameMap = GameMap.CreatePhase2Default();

        var result = gameMap.TryMove(RoomDirection.Up);

        Assert.True(result);
    }

    [Fact]
    public void TryMove_未接続方向へRoom移動できない()
    {
        var gameMap = GameMap.CreatePhase2Default();
        gameMap.TryMove(RoomDirection.Up);

        var result = gameMap.TryMove(RoomDirection.Up);

        Assert.False(result);
        Assert.Equal("Abandoned Line", gameMap.CurrentRoom.Name);
    }

    [Fact]
    public void TryMove_Room移動後にCurrentRoomが変わる()
    {
        var gameMap = GameMap.CreatePhase2Default();

        gameMap.TryMove(RoomDirection.Right);

        Assert.Equal("Fortress Gate", gameMap.CurrentRoom.Name);
    }

    [Fact]
    public void TryMove_Room移動後にIsVisitedがtrueになる()
    {
        var gameMap = GameMap.CreatePhase2Default();

        gameMap.TryMove(RoomDirection.Left);

        Assert.True(gameMap.CurrentRoom.IsVisited);
    }

    [Fact]
    public void ToggleMiniMap_ミニマップ表示フラグを切り替えられる()
    {
        var gameMap = GameMap.CreatePhase2Default();

        gameMap.ToggleMiniMap();

        Assert.True(gameMap.IsMiniMapVisible);
    }
}
