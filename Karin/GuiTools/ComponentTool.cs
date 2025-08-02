using System.Reflection;
using DefaultEcs;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Karin.GuiTools;

public class ComponentTool : ToolBase
{
    private object? _component;
    private Entity _entity;
    private bool _modified = false;

    public object SetComponent(object component) => _component = component;
    public void SetEntity(Entity entity) => _entity = entity;

    public ComponentTool(bool active = true) : base(active) {}

    public override void Draw()
    {

        if (_component == null)
        {
            ImGui.Begin("Component Tool", ref Active);
            ImGui.Text("Component not found");
            ImGui.End();
            return;
        }

        Type type = _component.GetType();
        var name = type.Name;
        ImGui.Begin($"{name}", ref Active);

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var attribute = field.GetCustomAttribute<ToolSerializeFieldAttribute>();
            if (attribute == null)
                continue;

            var label = attribute.Label ?? field.Name;

            object? value = field.GetValue(_component);

            if(value is Vector2 vector2Value)
            {
                DebugHelper.Log($"Vector2: {vector2Value.X}, {vector2Value.Y}");
                System.Numerics.Vector2 vector2 = new System.Numerics.Vector2(vector2Value.X, vector2Value.Y);
                if (ImGui.InputFloat2($"{label}##{field.Name}", ref vector2))
                {
                    field.SetValue(_component, vector2Value);
                    if (vector2Value.X != vector2.X || vector2Value.Y != vector2.Y)
                        _modified = true;
                }
            }
            // else if(value is int intValue)
            // {
            //     if (ImGui.InputInt($"{label}##{field.Name}", ref intValue))
            //     {
            //         field.SetValue(_component, intValue);
            //         _modified = true;
            //     }

            // }
            // else if(value is float floatValue)
            // {
            //     if (ImGui.InputFloat($"{label}##{field.Name}", ref floatValue))
            //     {
            //         field.SetValue(_component, floatValue);
            //         _modified = true;
            //     }
            // }
            // else if(value is bool boolValue)
            //     if (ImGui.Checkbox($"{label}##{field.Name}", ref boolValue))
            //     {
            //         field.SetValue(_component, boolValue);
            //         _modified = true;
            //     }
            // else if(value is string stringValue)
            // {
            //     if (ImGui.InputText($"{label}##{field.Name}", ref stringValue, 100))
            //     {
            //         field.SetValue(_component, stringValue);
            //         _modified = true;
            //     }
            // }


            if(_modified)
            {
                DebugHelper.Log($"{label}: {value.GetType().Name}");
                _entity.Set(_component);
                _modified = false;
            }
        }
        ImGui.End();

    }
}
