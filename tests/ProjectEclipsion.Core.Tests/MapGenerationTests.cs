using System.Collections.Generic;
using System.Linq;
using ProjectEclipsion.Core.Gameplay.World.Generation;
using ProjectEclipsion.Core.Gameplay.World.Maps;
using ProjectEclipsion.Core.Gameplay.World.Rooms;
using Xunit;

namespace ProjectEclipsion.Core.Tests;

public sealed class MapGenerationTests
{
    [Fact]
    public void BspMapGenerator_IMapGeneratorとして作成できる()
    {
        IMapGenerator generator = new BspMapGenerator(seed: 1);

        Assert.NotNull(generator);
    }

    [Fact]
    public void Generate_GameMapを生成できる()
    {
        var generator = new BspMapGenerator(seed: 1);

        var gameMap = generator.Generate();

        Assert.NotNull(gameMap);
    }

    [Fact]
    public void Generate_生成されたRoom数は5以上である()
    {
        var gameMap = new BspMapGenerator(seed: 1).Generate();

        Assert.True(gameMap.Rooms.Count >= 5);
    }

    [Fact]
    public void Generate_初期Roomが存在する()
    {
        var gameMap = new BspMapGenerator(seed: 1).Generate();

        Assert.NotNull(gameMap.CurrentRoom);
        Assert.Equal("Room 0", gameMap.CurrentRoom.Name);
    }

    [Fact]
    public void Generate_すべてのRoomが接続済みで到達可能である()
    {
        var gameMap = new BspMapGenerator(seed: 1).Generate();

        var visitedRoomIds = CollectReachableRoomIds(gameMap);

        Assert.Equal(gameMap.Rooms.Count, visitedRoomIds.Count);
    }

    [Fact]
    public void Generate_RoomがXY座標を持つ()
    {
        var gameMap = new BspMapGenerator(seed: 1).Generate();

        Assert.All(gameMap.Rooms, room =>
        {
            Assert.IsType<int>(room.X);
            Assert.IsType<int>(room.Y);
        });
    }

    [Fact]
    public void Generate_Room同士が上下左右で接続される()
    {
        var gameMap = new BspMapGenerator(seed: 1).Generate();

        foreach (var room in gameMap.Rooms)
        {
            foreach (var connection in room.Connections)
            {
                var connectedRoom = gameMap.Rooms.First(candidate => candidate.Id == connection.Value);

                Assert.True(System.Enum.IsDefined(typeof(RoomDirection), connection.Key));
                Assert.True(IsAdjacentInDirection(room, connectedRoom, connection.Key));
            }
        }
    }

    [Fact]
    public void Generate_初期RoomにEnemyCountは0である()
    {
        var gameMap = new BspMapGenerator(seed: 1).Generate();

        Assert.Equal(0, gameMap.CurrentRoom.EnemyCount);
    }

    [Fact]
    public void Generate_初期RoomにTreasureChestCountは0である()
    {
        var gameMap = new BspMapGenerator(seed: 1).Generate();

        Assert.Equal(0, gameMap.CurrentRoom.TreasureChestCount);
    }

    [Fact]
    public void Generate_初期Room以外にEnemyCountを設定できる()
    {
        var gameMap = new BspMapGenerator(seed: 1).Generate();

        Assert.Contains(gameMap.Rooms.Where(room => room.Id != gameMap.CurrentRoom.Id), room => room.EnemyCount > 0);
    }

    [Fact]
    public void Generate_初期Room以外にTreasureChestCountを設定できる()
    {
        var gameMap = new BspMapGenerator(seed: 1).Generate();

        Assert.Contains(gameMap.Rooms.Where(room => room.Id != gameMap.CurrentRoom.Id), room => room.TreasureChestCount > 0);
    }

    private static HashSet<string> CollectReachableRoomIds(GameMap gameMap)
    {
        var visitedRoomIds = new HashSet<string>();
        var queue = new Queue<Room>();
        queue.Enqueue(gameMap.CurrentRoom);

        while (queue.Count > 0)
        {
            var room = queue.Dequeue();
            if (!visitedRoomIds.Add(room.Id))
            {
                continue;
            }

            foreach (var nextRoomId in room.Connections.Values)
            {
                var nextRoom = gameMap.Rooms.First(candidate => candidate.Id == nextRoomId);
                queue.Enqueue(nextRoom);
            }
        }

        return visitedRoomIds;
    }

    private static bool IsAdjacentInDirection(Room from, Room to, RoomDirection direction)
    {
        return direction switch
        {
            RoomDirection.Up => to.X == from.X && to.Y == from.Y - 1,
            RoomDirection.Down => to.X == from.X && to.Y == from.Y + 1,
            RoomDirection.Left => to.X == from.X - 1 && to.Y == from.Y,
            RoomDirection.Right => to.X == from.X + 1 && to.Y == from.Y,
            _ => false,
        };
    }
}
