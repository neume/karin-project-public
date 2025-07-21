using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karin;

public struct TileMapComponent
{
    public Dictionary<Vector2, int> Map;
    public Dictionary<int, Rectangle> TextureStore;
    public Texture2D? TextureAtlas;
    public Vector2 Position;
    public bool IsVisible;
    public int TileWidth;
    public int TileHeight;

    public TileMapComponent(){
        IsVisible = false;
        Position = new Vector2(0, 0);
        TileWidth = 32;
        TileHeight = 32;
        TextureStore = new Dictionary<int, Rectangle>();
        Map = new Dictionary<Vector2, int>();
        TextureAtlas = null;
    }
}
