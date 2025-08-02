using ImGuiNET;
using DefaultEcs;
using Karin.Components;

namespace Karin.GuiTools;

public class EntityTool : ToolBase
{
    public EntityTool(bool active = false) : base(active) {}

    public override void Draw()
    {
        var set = Application.Instance.CurrentScene.World.GetEntities().With<TagComponent>().AsSet();

        ImGui.Begin("Entities", ref Active);
        ImGui.Text($"Count: {set.Count}");

        ListEntities(set);

        ImGui.End();
    }

    public void ListEntities(EntitySet set)
    {

        Span<Entity> entities = stackalloc Entity[set.Count];
        set.GetEntities().CopyTo(entities);

        int counter = 0;
        foreach (ref readonly Entity entity in entities)
        {
            var name = entity.Get<TagComponent>().Name;


            if (ImGui.CollapsingHeader($"{name}##{counter}"))
            {
                if(ImGui.CollapsingHeader($"Components##{counter}", ImGuiTreeNodeFlags.DefaultOpen))
                {
                    ListComponents(entity, counter);
                }

                if(ImGui.Button($"Delete##{counter}"))
                {
                    entity.Dispose();
                }
            }
            counter++;
        }
    }

    private void ListComponents(Entity entity, int id)
    {
        foreach (var type in AppGlobals.GetInspectableTypes())
        {
            var hasMethod = typeof(Entity).GetMethod("Has")!.MakeGenericMethod(type);

            if (!(bool)hasMethod.Invoke(entity, null)!) continue;

            var getMethod = typeof(Entity).GetMethod("Get")!.MakeGenericMethod(type);
            var component = getMethod.Invoke(entity, null);

            string componentName = component.GetType().Name;

            if(ImGui.Button($"{componentName}##{id}"))
            {
                var componentTool = new ComponentTool(true);
                componentTool.SetComponent(component);
                componentTool.SetEntity(entity);
                ToolManager.Add(componentTool);
            }
        }
    }
}
