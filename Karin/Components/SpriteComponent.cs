using Microsoft.Xna.Framework;
using Karin.GuiTools;
namespace Karin.Components;

[ToolInspectable]
public struct SpriteComponent
{
    [ToolSerializeField("SpriteName", false)]
    public string SpriteName;

    public Rectangle SourceRectangle;

    [ToolSerializeField("Position")]
    public Vector2 Position;
}
