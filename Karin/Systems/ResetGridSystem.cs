using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Karin.Spatials;

namespace Karin.Systems;

public class ResetGridSystem : ISystem<GameTime>
{
    public bool IsEnabled { get; set; } = true;
    private GridPartitioning Grid;

    public ResetGridSystem(GridPartitioning grid)
    {
        Grid = grid;
    }

    public void Dispose()
    {
    }

    public void Update(GameTime state)
    {
        Grid.Clear();
    }
}
