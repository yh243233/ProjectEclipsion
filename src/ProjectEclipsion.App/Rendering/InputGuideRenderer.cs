using System.Collections.Generic;

namespace ProjectEclipsion.App.Rendering;

public sealed class InputGuideRenderer
{
    public IReadOnlyList<string> BuildLines()
    {
        return new[]
        {
            "操作: WASD移動 / Space射撃 / Tダメージ / 1-6武器切替",
            "      G取得 / E装備 / Mミニマップ / Q終了",
        };
    }
}
