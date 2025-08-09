using ECS = DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Karin.Components;

namespace Karin.Systems;

public class TilemapFileLoaderSystem : AEntitySetSystem<GameTime>
{
    public TilemapFileLoaderSystem( ECS.World world)
        : base(world.GetEntities().With<TileMapComponent>().AsSet())
    {
    }

    protected override void Update(GameTime gameTime, in ECS.Entity entity)
    {
        ref var tileMapComponent = ref entity.Get<TileMapComponent>();

        if (tileMapComponent.MapPath == null)
            return;


        if (tileMapComponent.Loaded)
            return;

        tileMapComponent.Map = Util.LoadTileMap(tileMapComponent.MapPath);

        tileMapComponent.Loaded = true;
    }
}
