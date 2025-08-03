namespace Karin.GuiTools;

public class ToolBase
{
    public bool Active;
    public string GroupId;
    protected ToolManager? ToolManager;

    public ToolBase(bool active = false) {
        Active = active;
        GroupId = GetType().Name;
    }

    public ToolBase(ToolManager toolManager, bool active = false)
        : this(active)
    {
        Active = active;
        ToolManager = toolManager;
    }

    public void Render()
    {
        if(Active)
        {
            Draw();
        }
        else
        {
            Remove();
        }
    }

    public virtual void Draw()
    {
    }

    public void Remove()
    {
        ToolManager?.Remove(this);
    }

    public string SetGroupId(string groupId) => GroupId = groupId;

    public void SetActive(bool active) => Active = active;

    public void SetToolManager(ToolManager toolManager) => ToolManager = toolManager;
}
