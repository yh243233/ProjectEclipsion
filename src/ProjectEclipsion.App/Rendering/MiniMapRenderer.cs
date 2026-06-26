using System.Collections.Generic;
using System.Linq;
using ProjectEclipsion.Core.Gameplay.World.Maps;

namespace ProjectEclipsion.App.Rendering;

public sealed class MiniMapRenderer
{
    public IReadOnlyList<string> BuildLines(GameMap gameMap)
    {
        var minX = gameMap.Rooms.Min(room => room.X);
        var maxX = gameMap.Rooms.Max(room => room.X);
        var minY = gameMap.Rooms.Min(room => room.Y);
        var maxY = gameMap.Rooms.Max(room => room.Y);

        var lines = new List<string>();
        for (var y = minY; y <= maxY; y++)
        {
            var cells = Enumerable.Range(minX, maxX - minX + 1)
                .Select(x => FormatCell(gameMap, x, y));
            lines.Add(string.Join(string.Empty, cells));
        }

        return lines;
    }

    private static string FormatCell(GameMap gameMap, int x, int y)
    {
        var room = gameMap.FindRoomAt(x, y);
        if (room is null)
        {
            return "[ ]";
        }

        if (room == gameMap.CurrentRoom)
        {
            return "[P]";
        }

        return room.IsVisited ? "[V]" : "[?]";
    }
}
