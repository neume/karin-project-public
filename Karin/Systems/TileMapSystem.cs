using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;

namespace Karin;


public class TileMapSystem : AEntitySetSystem<float>
{
    public TileMapSystem(World world)
        : base(world.GetEntities().With<TileMapComponent>().AsSet())
    {
    }

    protected override void Update(float state, in Entity entity)
    {
        var tileMapComponent = entity.Get<TileMapComponent>();

        DrawTileMap(tileMapComponent);
    }

    // Private Methods

    private void DrawTileMap(TileMapComponent tileMapComponent)
    {
        if (!tileMapComponent.IsVisible)
            return;

        foreach (var item in tileMapComponent.Map)
        {
            Rectangle dest = new Rectangle(
                ((int)item.Key.X * tileMapComponent.TileWidth),
                ((int)item.Key.Y * tileMapComponent.TileHeight),
                tileMapComponent.TileWidth,
                tileMapComponent.TileHeight
            );
            Rectangle src = GetTileRectangle(item.Value, tileMapComponent);
            AppGlobals.Renderer.Draw(tileMapComponent.TextureAtlas, src, dest, Color.White);
        }
    }

    private Rectangle GetTileRectangle(int index, TileMapComponent tileMapComponent)
    {
        if(tileMapComponent.TextureStore.ContainsKey(index))
            return tileMapComponent.TextureStore[index];

        Console.WriteLine($"TileMapSystem: Loading tile {index}");

        var rectangle = CalculateRectangle(index,tileMapComponent);
        tileMapComponent.TextureStore.Add(index, rectangle);
        return rectangle;
    }

    private Rectangle CalculateRectangle(int index, TileMapComponent tileMapComponent)
    {
        int colCount = tileMapComponent.TextureAtlas.Width / tileMapComponent.TileWidth;
        int rowCount = tileMapComponent.TextureAtlas.Height / tileMapComponent.TileHeight;

        int col = index % colCount;
        int row = index / colCount;

        return new Rectangle(col * tileMapComponent.TileWidth,
                row * tileMapComponent.TileHeight,
                tileMapComponent.TileWidth,
                tileMapComponent.TileHeight);
    }
}
