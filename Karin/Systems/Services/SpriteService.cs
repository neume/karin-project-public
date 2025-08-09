using ECS = DefaultEcs;
using Karin.Components;
using Karin.Graphics;
using Microsoft.Xna.Framework;

namespace Karin.Systems.Services;

public class SpriteService : ISystemService
{

    public void Execute(GameTime gameTime, ECS.Entity entity)
    {
        if(!entity.Has<SpriteComponent>())
            return;

        drawSprite(entity);
    }

    private void drawSprite(in ECS.Entity entity)
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
}
