using DefaultEcs;
using DefaultEcs.System;
using Karin.Components;
using Karin.Spatials;
using Microsoft.Xna.Framework;

namespace Karin.Systems;

public class PopulateGridSystem : AEntitySetSystem<GameTime>
{
    public GridPartitioning Grid { get; private set; }

    public PopulateGridSystem(World world, GridPartitioning grid)
        : base(world.GetEntities()
                .With<SpatialComponent>()
                .With<IdentityComponent>()
                .With<TransformComponent>()
                .AsSet())
    {
        Grid = grid;
    }

    protected override void Update(GameTime gameTime, in Entity entity)
    {
        var transform = entity.Get<TransformComponent>();

        Grid.Add(transform.Position.X, transform.Position.Y, entity);
    }
}
