using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Karin.GuiTools;

namespace Karin.Components;

[ToolInspectable]
public struct TransformComponent
{
    [ToolSerializeField("Position")]
    public Vector2 Position;

    [ToolSerializeField("Rotation")]
    public float Rotation;

    [ToolSerializeField("Scale")]
    public Vector2 Scale;

    public TransformComponent()
    {
        Position = new Vector2(0, 0);
        Rotation = 0;
        Scale = new Vector2(1, 1);
    }
}
