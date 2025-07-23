using Microsoft.Xna.Framework;

namespace Karin.TileMaps;

public static class TileMap
{
    public static void DrawTileMap(TileMapComponent tileMapComponent, float zIndex = 0)
    {
        if (!tileMapComponent.IsVisible)
            return;

        foreach (var item in tileMapComponent.Map)
        {
            Rectangle dest = new Rectangle(
                ((int)item.Key.X * tileMapComponent.TileWidth) + (int)tileMapComponent.Position.X,
                ((int)item.Key.Y * -tileMapComponent.TileHeight) + (int)tileMapComponent.Position.Y,
                tileMapComponent.TileWidth,
                tileMapComponent.TileHeight
            );
            Rectangle src = GetTileRectangle(item.Value, tileMapComponent);
            AppGlobals.Renderer.Draw(tileMapComponent.TextureAtlas, src, dest, Color.White, zIndex);
        }
    }

    private static Rectangle GetTileRectangle(int index, TileMapComponent tileMapComponent)
    {
        if(tileMapComponent.TextureStore.ContainsKey(index))
            return tileMapComponent.TextureStore[index];

        var rectangle = CalculateRectangle(index,tileMapComponent);
        tileMapComponent.TextureStore.Add(index, rectangle);
        return rectangle;
    }

    private static Rectangle CalculateRectangle(int index, TileMapComponent tileMapComponent)
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
