using ProjectEclipsion.Core.Gameplay.World.Maps;

namespace ProjectEclipsion.Core.Gameplay.World.Generation;

public interface IMapGenerator
{
    GameMap Generate();
}
