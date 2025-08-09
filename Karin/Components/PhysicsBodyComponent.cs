using Karin.GuiTools;
using Microsoft.Xna.Framework;

[ToolInspectable]
public struct PhysicsBodyComponent
{
    [ToolSerializeField("Is Kinematic")]
    public bool IsKinematic;

    [ToolSerializeField("Mass")]
    public float Mass;

    [ToolSerializeField("Velocity")]
    public Vector2 Velocity;

    [ToolSerializeField("Reference Body", false)]
    public int? ReferenceBody;

    public float? Restitution;
    public float? Friction;

}
