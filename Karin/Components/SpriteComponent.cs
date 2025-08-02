using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Karin.GuiTools;
namespace Karin.Components;

[ToolInspectable]
public struct SpriteComponent
{
    // [ToolSerializeField("Texture")]
    public Texture2D Texture;

    // [ToolSerializeField("Source Rectangle")]
    public Rectangle SourceRectangle;

    [ToolSerializeField("Position")]
    public Vector2 Position;
}
