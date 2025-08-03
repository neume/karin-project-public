using Microsoft.Xna.Framework;
using Karin.GuiTools;

namespace Karin.Components;

[ToolInspectable]
public struct TileMapComponent
{
    public Dictionary<Vector2, int> Map;
    public Dictionary<int, Rectangle> TextureStore;
    public string TextureName;

    [ToolSerializeField("Visible")]
    public bool IsVisible;

    [ToolSerializeField("Tile Width")]
    public int TileWidth;

    [ToolSerializeField("Tile Height")]
    public int TileHeight;

    public TileMapComponent(){
        IsVisible = false;
        TileWidth = 32;
        TileHeight = 32;
        TextureStore = new Dictionary<int, Rectangle>();
        Map = new Dictionary<Vector2, int>();
    }
}
