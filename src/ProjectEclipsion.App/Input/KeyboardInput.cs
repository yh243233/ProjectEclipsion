using System;

namespace ProjectEclipsion.App.Input;

public sealed class KeyboardInput
{
    public (int DirectionX, int DirectionY, bool ShouldExit) ReadDirection()
    {
        if (Console.IsInputRedirected)
        {
            var input = Console.In.Read();
            return input < 0
                ? (0, 0, true)
                : ToDirection((char)input);
        }

        var key = Console.ReadKey(intercept: true).Key;

        return key switch
        {
            ConsoleKey.W => (0, -1, false),
            ConsoleKey.S => (0, 1, false),
            ConsoleKey.A => (-1, 0, false),
            ConsoleKey.D => (1, 0, false),
            ConsoleKey.Q => (0, 0, true),
            ConsoleKey.Escape => (0, 0, true),
            _ => (0, 0, false),
        };
    }

    private static (int DirectionX, int DirectionY, bool ShouldExit) ToDirection(char input)
    {
        return char.ToUpperInvariant(input) switch
        {
            'W' => (0, -1, false),
            'S' => (0, 1, false),
            'A' => (-1, 0, false),
            'D' => (1, 0, false),
            'Q' => (0, 0, true),
            _ => (0, 0, false),
        };
    }
}
