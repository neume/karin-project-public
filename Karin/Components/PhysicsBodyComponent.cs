using Karin.GuiTools;
using Microsoft.Xna.Framework;

[ToolInspectable]
public struct PhysicsBodyComponent
{
    [ToolSerializeField("Body", false)]
    public string Body;

    [ToolSerializeField("Is Kinematic")]
    public bool IsKinematic;

    [ToolSerializeField("Mass")]
    public float Mass;

    [ToolSerializeField("Velocity")]
    public Vector2 Velocity;
}
