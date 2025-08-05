using DefaultEcs;
using DefaultEcs.System;
using Karin.Components;
using Karin.Graphics;
using Karin.TileMaps;
using Microsoft.Xna.Framework;

namespace Karin.Systems;

public class DrawSystem : AEntitySetSystem<GameTime>
{
    public DrawSystem(World world)
        : base(world.GetEntities().With<DrawInfoComponent>().AsSet())
    {
    }

    protected override void Update(GameTime gameTime, in Entity entity)
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
        if (!entity.Has<TransformComponent>())
            return;

        var transformComponent = entity.Get<TransformComponent>();
        var drawInfoComponent = entity.Get<DrawInfoComponent>();

        if (!drawInfoComponent.IsVisible)
            return;

        var texture = TextureManager.GetTexture(spriteComponent.SpriteName);

        if (texture == null)
            return;

        AppGlobals.Renderer.Draw(texture,
                                transformComponent.Position,
                                spriteComponent.SourceRectangle,
                                drawInfoComponent.ZIndex);
    }

    private void drawTileMap(in Entity entity)
    {
        var tileMapComponent = entity.Get<TileMapComponent>();
        var drawInfoComponent = entity.Get<DrawInfoComponent>();

        if (!drawInfoComponent.IsVisible)
            return;

        if(!entity.Has<TransformComponent>())
            return;
        var transformComponent = entity.Get<TransformComponent>();

        TileMap.DrawTileMap(tileMapComponent, transformComponent, drawInfoComponent.ZIndex);
    }
}
