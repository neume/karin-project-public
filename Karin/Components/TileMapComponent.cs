using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Karin.GuiTools;

namespace Karin.Components;

[ToolInspectable]
public struct TileMapComponent
{
    public Dictionary<Vector2, int> Map;
    public Dictionary<int, Rectangle> TextureStore;
    public Texture2D TextureAtlas;
    public Vector2 Position;
    [ToolSerializeField("Visible")]
    public bool IsVisible;
    [ToolSerializeField("Tile Width")]
    public int TileWidth;
    [ToolSerializeField("Tile Height")]
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
