using System;
using System.Collections.Generic;
using ProjectEclipsion.Core.Gameplay.World.Biomes;

namespace ProjectEclipsion.Core.Gameplay.World.Rooms;

public sealed class Room
{
    private readonly Dictionary<RoomDirection, string> connections = new();

    public Room(string id, string name, int x, int y, BiomeType biomeType)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Room IDは必須です。", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Room名は必須です。", nameof(name));
        }

        Id = id;
        Name = name;
        X = x;
        Y = y;
        BiomeType = biomeType;
    }

    public string Id { get; }

    public string Name { get; }

    public int X { get; }

    public int Y { get; }

    public BiomeType BiomeType { get; }

    public bool IsVisited { get; private set; }

    public IReadOnlyDictionary<RoomDirection, string> Connections => connections;

    public void Connect(RoomDirection direction, string roomId)
    {
        if (string.IsNullOrWhiteSpace(roomId))
        {
            throw new ArgumentException("接続先Room IDは必須です。", nameof(roomId));
        }

        connections[direction] = roomId;
    }

    public bool TryGetConnectedRoomId(RoomDirection direction, out string roomId)
    {
        return connections.TryGetValue(direction, out roomId!);
    }

    public void MarkVisited()
    {
        IsVisited = true;
    }
}
