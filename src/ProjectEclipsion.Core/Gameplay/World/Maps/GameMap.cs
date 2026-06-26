using System;
using System.Collections.Generic;
using System.Linq;
using ProjectEclipsion.Core.Gameplay.World.Biomes;
using ProjectEclipsion.Core.Gameplay.World.Rooms;

namespace ProjectEclipsion.Core.Gameplay.World.Maps;

public sealed class GameMap
{
    private readonly List<Room> rooms;

    public GameMap(IEnumerable<Room> rooms, string initialRoomId)
    {
        this.rooms = rooms?.ToList() ?? throw new ArgumentNullException(nameof(rooms));

        CurrentRoom = this.rooms.FirstOrDefault(room => room.Id == initialRoomId)
            ?? throw new ArgumentException("初期Roomが見つかりません。", nameof(initialRoomId));
        CurrentRoom.MarkVisited();
    }

    public IReadOnlyList<Room> Rooms => rooms;

    public Room CurrentRoom { get; private set; }

    public bool IsMiniMapVisible { get; private set; }

    public bool TryMove(RoomDirection direction)
    {
        if (!CurrentRoom.TryGetConnectedRoomId(direction, out var nextRoomId))
        {
            return false;
        }

        var nextRoom = rooms.FirstOrDefault(room => room.Id == nextRoomId);
        if (nextRoom is null)
        {
            return false;
        }

        CurrentRoom = nextRoom;
        CurrentRoom.MarkVisited();
        return true;
    }

    public void ToggleMiniMap()
    {
        IsMiniMapVisible = !IsMiniMapVisible;
    }

    public Room? FindRoomAt(int x, int y)
    {
        return rooms.FirstOrDefault(room => room.X == x && room.Y == y);
    }

    public static GameMap CreatePhase2Default()
    {
        var central = new Room("central", "Central Hub", x: 0, y: 0, BiomeType.UndergroundFacility);
        var north = new Room("north_factory", "Abandoned Line", x: 0, y: -1, BiomeType.AbandonedFactory);
        var south = new Room("south_lab", "Lower Laboratory", x: 0, y: 1, BiomeType.Laboratory);
        var west = new Room("west_snowfield", "Frozen Access", x: -1, y: 0, BiomeType.Snowfield);
        var east = new Room("east_fortress", "Fortress Gate", x: 1, y: 0, BiomeType.Fortress);
        var otherworld = new Room("otherworld_rift", "Silent Rift", x: 1, y: -1, BiomeType.Otherworld);

        ConnectBoth(central, RoomDirection.Up, north, RoomDirection.Down);
        ConnectBoth(central, RoomDirection.Down, south, RoomDirection.Up);
        ConnectBoth(central, RoomDirection.Left, west, RoomDirection.Right);
        ConnectBoth(central, RoomDirection.Right, east, RoomDirection.Left);
        ConnectBoth(east, RoomDirection.Up, otherworld, RoomDirection.Down);

        return new GameMap(
            new[] { central, north, south, west, east, otherworld },
            initialRoomId: central.Id);
    }

    private static void ConnectBoth(Room from, RoomDirection direction, Room to, RoomDirection oppositeDirection)
    {
        from.Connect(direction, to.Id);
        to.Connect(oppositeDirection, from.Id);
    }
}
