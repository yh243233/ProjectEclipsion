using System;
using ProjectEclipsion.Core.Rendering;

namespace ProjectEclipsion.Core;

public sealed class GameLoop
{
    private readonly GameState gameState;
    private readonly IRenderer renderer;

    public GameLoop(GameState gameState, IRenderer renderer)
    {
        this.gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
        this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    }

    public void RunOnce()
    {
        renderer.Render(gameState);
    }
}
