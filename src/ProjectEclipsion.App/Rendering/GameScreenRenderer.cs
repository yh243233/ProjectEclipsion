using System;
using System.Collections.Generic;
using System.Linq;
using ProjectEclipsion.Core;

namespace ProjectEclipsion.App.Rendering;

public sealed class GameScreenRenderer
{
    public const int DefaultWidth = 28;
    public const int DefaultHeight = 10;

    public IReadOnlyList<string> BuildLines(GameState gameState, int width = DefaultWidth, int height = DefaultHeight)
    {
        ArgumentNullException.ThrowIfNull(gameState);

        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be positive.");
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be positive.");
        }

        var cells = CreateEmptyMap(width, height);
        var centerX = width / 2;
        var centerY = height / 2;

        var mapper = new ViewportMapper(gameState, width, height);

        foreach (var bullet in gameState.Bullets)
        {
            if (mapper.TryMap(bullet.X, bullet.Y, out var mapX, out var mapY))
            {
                TrySet(cells, width, height, mapX, mapY, '*');
            }
        }

        foreach (var enemy in gameState.Enemies)
        {
            if (mapper.TryMap(enemy.X, enemy.Y, out var mapX, out var mapY))
            {
                TrySet(cells, width, height, mapX, mapY, 'E');
            }
        }

        TrySet(cells, width, height, centerX, centerY, 'P');

        var lines = new List<string>();
        for (var y = 0; y < height; y++)
        {
            lines.Add(new string(cells[y]));
        }

        return lines;
    }

    private static char[][] CreateEmptyMap(int width, int height)
    {
        var cells = new char[height][];
        for (var y = 0; y < height; y++)
        {
            cells[y] = new string('.', width).ToCharArray();
        }

        return cells;
    }

    private static void TrySet(char[][] cells, int width, int height, int x, int y, char marker)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return;
        }

        cells[y][x] = marker;
    }

    private sealed class ViewportMapper
    {
        private readonly GameState gameState;
        private readonly int width;
        private readonly int height;
        private readonly int centerX;
        private readonly int centerY;
        private readonly int minX;
        private readonly int maxX;
        private readonly int minY;
        private readonly int maxY;

        public ViewportMapper(GameState gameState, int width, int height)
        {
            this.gameState = gameState;
            this.width = width;
            this.height = height;
            centerX = width / 2;
            centerY = height / 2;

            var xValues = new List<int> { gameState.Player.X };
            var yValues = new List<int> { gameState.Player.Y };
            xValues.AddRange(gameState.Enemies.Select(enemy => enemy.X));
            yValues.AddRange(gameState.Enemies.Select(enemy => enemy.Y));
            xValues.AddRange(gameState.Bullets.Select(bullet => bullet.X));
            yValues.AddRange(gameState.Bullets.Select(bullet => bullet.Y));

            minX = xValues.Min();
            maxX = xValues.Max();
            minY = yValues.Min();
            maxY = yValues.Max();
        }

        public bool TryMap(int x, int y, out int mapX, out int mapY)
        {
            mapX = centerX + x - gameState.Player.X;
            mapY = centerY + y - gameState.Player.Y;
            if (IsInRange(mapX, mapY))
            {
                return true;
            }

            return TryMapCompressed(x, y, out mapX, out mapY);
        }

        private bool TryMapCompressed(int x, int y, out int mapX, out int mapY)
        {
            mapX = 0;
            mapY = 0;
            var spanX = maxX - minX;
            var spanY = maxY - minY;
            if (spanX > width * 2 || spanY > height * 2)
            {
                return false;
            }

            mapX = spanX == 0 ? centerX : (int)Math.Round((double)(x - minX) * (width - 1) / spanX);
            mapY = spanY == 0 ? centerY : (int)Math.Round((double)(y - minY) * (height - 1) / spanY);
            return IsInRange(mapX, mapY);
        }

        private bool IsInRange(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
    }
}
