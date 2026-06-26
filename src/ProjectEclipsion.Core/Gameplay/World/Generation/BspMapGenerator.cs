using System;
using System.Collections.Generic;
using System.Linq;
using ProjectEclipsion.Core.Gameplay.World.Biomes;
using ProjectEclipsion.Core.Gameplay.World.Maps;
using ProjectEclipsion.Core.Gameplay.World.Rooms;

namespace ProjectEclipsion.Core.Gameplay.World.Generation;

public sealed class BspMapGenerator : IMapGenerator
{
    private const int MinimumRoomCount = 5;
    private readonly Random random;
    private readonly int roomCount;

    public BspMapGenerator(int? seed = null, int roomCount = 7)
    {
        if (roomCount < MinimumRoomCount)
        {
            throw new ArgumentOutOfRangeException(nameof(roomCount), "生成Room数は5以上である必要があります。");
        }

        this.roomCount = roomCount;
        random = seed.HasValue ? new Random(seed.Value) : new Random();
    }

    public GameMap Generate()
    {
        var rooms = new List<Room>
        {
            new Room("room_0", "Room 0", x: 0, y: 0, BiomeType.UndergroundFacility),
        };
        var occupiedPositions = new HashSet<(int X, int Y)> { (0, 0) };

        while (rooms.Count < roomCount)
        {
            var candidates = rooms
                .SelectMany(room => GetAvailableDirections(room, occupiedPositions)
                    .Select(direction => (Room: room, Direction: direction)))
                .ToList();

            if (candidates.Count == 0)
            {
                break;
            }

            var selected = candidates[random.Next(candidates.Count)];
            var nextPosition = GetNextPosition(selected.Room.X, selected.Room.Y, selected.Direction);
            var room = new Room(
                $"room_{rooms.Count}",
                $"Room {rooms.Count}",
                nextPosition.X,
                nextPosition.Y,
                GetRandomBiome());

            ConnectBoth(selected.Room, selected.Direction, room, GetOppositeDirection(selected.Direction));
            rooms.Add(room);
            occupiedPositions.Add(nextPosition);
        }

        ApplyPlacements(rooms);
        return new GameMap(rooms, initialRoomId: rooms[0].Id);
    }

    private void ApplyPlacements(IReadOnlyList<Room> rooms)
    {
        var hasEnemy = false;
        var hasTreasure = false;

        for (var i = 1; i < rooms.Count; i++)
        {
            var enemyCount = random.Next(0, 4);
            var treasureChestCount = random.Next(0, 2);
            rooms[i].SetPlacementCounts(enemyCount, treasureChestCount);
            hasEnemy |= enemyCount > 0;
            hasTreasure |= treasureChestCount > 0;
        }

        if (rooms.Count <= 1)
        {
            return;
        }

        if (!hasEnemy)
        {
            rooms[1].SetPlacementCounts(enemyCount: 1, rooms[1].TreasureChestCount);
        }

        if (!hasTreasure)
        {
            rooms[1].SetPlacementCounts(rooms[1].EnemyCount, treasureChestCount: 1);
        }
    }

    private BiomeType GetRandomBiome()
    {
        var biomeTypes = Enum.GetValues<BiomeType>();
        return biomeTypes[random.Next(biomeTypes.Length)];
    }

    private static IEnumerable<RoomDirection> GetAvailableDirections(Room room, HashSet<(int X, int Y)> occupiedPositions)
    {
        foreach (var direction in Enum.GetValues<RoomDirection>())
        {
            var nextPosition = GetNextPosition(room.X, room.Y, direction);
            if (!occupiedPositions.Contains(nextPosition))
            {
                yield return direction;
            }
        }
    }

    private static (int X, int Y) GetNextPosition(int x, int y, RoomDirection direction)
    {
        return direction switch
        {
            RoomDirection.Up => (x, y - 1),
            RoomDirection.Down => (x, y + 1),
            RoomDirection.Left => (x - 1, y),
            RoomDirection.Right => (x + 1, y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "未対応のRoom方向です。"),
        };
    }

    private static RoomDirection GetOppositeDirection(RoomDirection direction)
    {
        return direction switch
        {
            RoomDirection.Up => RoomDirection.Down,
            RoomDirection.Down => RoomDirection.Up,
            RoomDirection.Left => RoomDirection.Right,
            RoomDirection.Right => RoomDirection.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "未対応のRoom方向です。"),
        };
    }

    private static void ConnectBoth(Room from, RoomDirection direction, Room to, RoomDirection oppositeDirection)
    {
        from.Connect(direction, to.Id);
        to.Connect(oppositeDirection, from.Id);
    }
}
