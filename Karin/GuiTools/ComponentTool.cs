using System.Reflection;
using DefaultEcs;
using ImGuiNET;
using Karin.Components;
using Microsoft.Xna.Framework;

namespace Karin.GuiTools;

public class ComponentTool : ToolBase
{
    private Type _componentType;
    private World _world;
    private string _entityId;

    public object SetComponentType(Type type) => _componentType = type;

    public ComponentTool(World world, string EntityId, bool active = true) : base(active)
    {
        _world = world;
        _entityId = EntityId;
    }

    public override void Draw()
    {
        var name = _componentType.Name;
        ImGui.Begin($"{name}", ref Active);

        var entity = FindEntity();

        var getMethod = typeof(Entity).GetMethod("Get")!.MakeGenericMethod(_componentType);

        object freshComponentBoxed = getMethod.Invoke(entity, null);

        if (freshComponentBoxed == null)
        {
            ImGui.Begin("Component Tool", ref Active);
            ImGui.Text("Component not found");
            ImGui.End();
            return;
        }

        var fieldInfos = _componentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


        foreach (var fieldInfo in fieldInfos)
        {
            var attribute = fieldInfo.GetCustomAttribute<ToolSerializeFieldAttribute>();
            if (attribute == null) continue;

            var label = attribute.Label ?? fieldInfo.Name;

            object? fieldValue = fieldInfo.GetValue(freshComponentBoxed);
            bool changed = false;

            if(fieldValue is Vector2 vector2Value)
                DisplayVector2(label, ref vector2Value, ref freshComponentBoxed, fieldInfo, entity,ref changed);
            else if(fieldValue is string stringValue)
                DisplayString(label, ref stringValue, ref freshComponentBoxed, fieldInfo, entity, ref changed);
            else if(fieldValue is int intValue)
                DisplayInt(label, ref intValue, ref freshComponentBoxed, fieldInfo, entity, ref changed);
            else if(fieldValue is float floatValue)
                DisplayFloat(label, ref floatValue, ref freshComponentBoxed, fieldInfo, entity, ref changed);
            else if(fieldValue is bool boolValue)
                DisplayBool(label, ref boolValue, ref freshComponentBoxed, fieldInfo, entity, ref changed);
            


            if(changed)
            {
                entity.Set(freshComponentBoxed);
            }
        }
        ImGui.End();
    }

    private void DisplayVector2(string label, ref Vector2 value, ref object component, FieldInfo fieldInfo, Entity entity, ref bool changed)
    {
        System.Numerics.Vector2 vector2 = new System.Numerics.Vector2(value.X, value.Y);

        if (ImGui.InputFloat2($"{label}##{fieldInfo.Name}", ref vector2))
        {
            var updatedValue = new Vector2(vector2.X, vector2.Y);


            object boxedComponent = component;

            fieldInfo.SetValue(boxedComponent, updatedValue);
            SetComponent(entity, boxedComponent);
        }
    }

    private void DisplayString(string label, ref string value, ref object component, FieldInfo fieldInfo, Entity entity, ref bool changed)
    {
        if (ImGui.InputText($"{label}##{fieldInfo.Name}", ref value, 100))
        {
            object boxedComponent = component;
            fieldInfo.SetValue(boxedComponent, value);
            SetComponent(entity, boxedComponent);
        }
    }

    private void DisplayInt(string label, ref int value, ref object component, FieldInfo fieldInfo, Entity entity, ref bool changed)
    {
        if (ImGui.InputInt($"{label}##{fieldInfo.Name}", ref value))
        {
            object boxedComponent = component;
            fieldInfo.SetValue(boxedComponent, value);
            SetComponent(entity, boxedComponent);
        }
    }

    private void DisplayFloat(string label, ref float value, ref object component, FieldInfo fieldInfo, Entity entity, ref bool changed)
    {
        if (ImGui.InputFloat($"{label}##{fieldInfo.Name}", ref value))
        {
            object boxedComponent = component;
            fieldInfo.SetValue(boxedComponent, value);
            SetComponent(entity, boxedComponent);
        }
    }

    private void DisplayBool(string label, ref bool value, ref object component, FieldInfo fieldInfo, Entity entity, ref bool changed)
    {
        if (ImGui.Checkbox($"{label}##{fieldInfo.Name}", ref value))
        {
            object boxedComponent = component;
            fieldInfo.SetValue(boxedComponent, value);
            SetComponent(entity, boxedComponent);
        }
    }

    private Entity FindEntity()
    {
        EntitySet set = _world.GetEntities().With<IdentityComponent>().AsSet();

        foreach (ref readonly Entity entity in set.GetEntities())
        {
            var identityComponent = entity.Get<IdentityComponent>();
            if (identityComponent.Id == _entityId)
            {
                return entity;
            }
        }

        return new Entity();
    }

    private void SetComponent(Entity entity, object component)
    {
        var setMethod = typeof(Entity).GetMethods()
            .First(m => m.Name == "Set" && m.GetParameters().Length == 1)
            .MakeGenericMethod(component.GetType());
        setMethod.Invoke(entity, new object[] { component});
    }
}
