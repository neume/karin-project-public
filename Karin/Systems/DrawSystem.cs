using System.Numerics;
using DefaultEcs;
using DefaultEcs.System;
using Karin.Components;
using Karin.TileMaps;

namespace Karin.Systems;

public class DrawSystem : AEntitySetSystem<float>
{
    public DrawSystem(World world)
        : base(world.GetEntities().With<DrawInfoComponent>().AsSet())
    {
    }

    protected override void Update(float state, in Entity entity)
    {
        if (entity.Has<SpriteComponent>())
        {
            drawSprite(entity);
        }

        if (entity.Has<TileMapComponent>())
        {
            drawTileMap(entity);
        }        
    }

    private void drawSprite(in Entity entity)
    {
        var spriteComponent = entity.Get<SpriteComponent>();
        var drawInfoComponent = entity.Get<DrawInfoComponent>();

        AppGlobals.Renderer.Draw(spriteComponent.Texture,
                                spriteComponent.Position,
                                spriteComponent.SourceRectangle,
                                drawInfoComponent.ZIndex);
    }

    private void drawTileMap(in Entity entity)
    {
        var tileMapComponent = entity.Get<TileMapComponent>();
        var drawInfoComponent = entity.Get<DrawInfoComponent>();

        TileMap.DrawTileMap(tileMapComponent, drawInfoComponent.ZIndex); 
    }
}
