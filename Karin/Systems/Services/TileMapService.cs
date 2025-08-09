using ECS = DefaultEcs;
using Microsoft.Xna.Framework;
using Karin.Components;
using Karin.Graphics;

namespace Karin.Systems.Services;

public class TileMapService : ISystemService
{

    public void Execute(GameTime gameTime, ECS.Entity entity)
    {
        if(!entity.Has<TileMapComponent>())
            return;

        ref var tileMapComponent = ref entity.Get<TileMapComponent>();

        var drawInfoComponent = entity.Get<DrawInfoComponent>();

        if (!drawInfoComponent.IsVisible)
            return;

        if(!entity.Has<TransformComponent>())
            return;

        ref var transformComponent = ref entity.Get<TransformComponent>();

        DrawTileMap(ref tileMapComponent, ref transformComponent, drawInfoComponent.ZIndex);
    }

    public void DrawTileMap(ref TileMapComponent tileMapComponent, ref TransformComponent transformComponent, float zIndex = 0)
    {
        if (!tileMapComponent.IsVisible)
            return;

        var textureAtlas = TextureManager.GetTexture(tileMapComponent.TextureName);

        if (textureAtlas == null)
            return;

        int colCount = textureAtlas.Width / tileMapComponent.TileWidth;
        int rowCount = textureAtlas.Height / tileMapComponent.TileHeight;

        int X = (int)transformComponent.Position.X;
        int Y = (int)transformComponent.Position.Y;

        foreach (var item in tileMapComponent.Map)
        {
            Rectangle dest = new Rectangle(
                ((int)item.Key.X * tileMapComponent.TileWidth) + X,
                ((int)item.Key.Y * -tileMapComponent.TileHeight) + Y,
                tileMapComponent.TileWidth,
                tileMapComponent.TileHeight
            );
            Rectangle src = GetTileRectangle(item.Value, tileMapComponent, colCount, rowCount);
            AppGlobals.Renderer.Draw(textureAtlas, src, dest, Color.White, zIndex);
        }
    }

    private Rectangle GetTileRectangle(int index, TileMapComponent tileMapComponent, int colCount, int rowCount)
    {
        if(tileMapComponent.TextureStore.ContainsKey(index))
            return tileMapComponent.TextureStore[index];

        var rectangle = CalculateRectangle(index,tileMapComponent, colCount, rowCount);
        tileMapComponent.TextureStore.Add(index, rectangle);
        return rectangle;
    }

    private Rectangle CalculateRectangle(int index, TileMapComponent tileMapComponent, int colCount, int rowCount)
    {
        int col = index % colCount;
        int row = index / colCount;

        return new Rectangle(col * tileMapComponent.TileWidth,
                row * tileMapComponent.TileHeight,
                tileMapComponent.TileWidth,
                tileMapComponent.TileHeight);
    }
}
