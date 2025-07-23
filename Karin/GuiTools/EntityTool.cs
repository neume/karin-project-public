using ImGuiNET;
using DefaultEcs;
using Karin.Components;

namespace Karin.GuiTools;

public class EntityTool : ImGuiTool
{
    public EntityTool(bool active = false) : base(active) {}

    public override void Render()
    {
        var set = Application.Instance.CurrentScene.World.GetEntities().With<TagComponent>().AsSet();

        if(Active)
        {
            ImGui.Begin("Entities", ref Active);
            ImGui.Text($"Count: {set.Count}");

            ListEntities(set);

            ImGui.End();
        }
    }

    public void ListEntities(EntitySet set)
    {

        Span<Entity> entities = stackalloc Entity[set.Count];
        set.GetEntities().CopyTo(entities);

        int counter = 0;
        foreach (ref readonly Entity entity in entities)
        {
            var name = entity.Get<TagComponent>().Name;

            if (ImGui.CollapsingHeader($"{name}"))
            {
                if(entity.Has<DrawInfoComponent>())
                {
                    var drawInfoComponent = entity.Get<DrawInfoComponent>();
                    float zIndex = drawInfoComponent.ZIndex;
                    if (ImGui.InputFloat($"ZIndex##{counter}", ref zIndex))
                    {
                        drawInfoComponent.ZIndex = zIndex;
                        entity.Set(drawInfoComponent);
                    }
                }
                if(ImGui.Button($"Delete##{counter}"))
                {
                    entity.Dispose();
                }
            }
            counter++;
        }
    }
}
