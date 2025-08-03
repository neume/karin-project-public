using System.Collections;
namespace Karin.GuiTools;

public class ToolManager : IEnumerable
{
    public List<ToolBase> Tools { get; private set; }
    private List<ToolBase> ToBeRemoved = new List<ToolBase>();
    private List<ToolBase> ToBeAdded = new List<ToolBase>();
    private Dictionary<string, ToolBase> _toolIds = new Dictionary<string, ToolBase>();

    public ToolManager()
    {
        Tools = new List<ToolBase>();
        ToBeRemoved = new List<ToolBase>();
        ToBeAdded = new List<ToolBase>();
        _toolIds = new Dictionary<string, ToolBase>();
    }

    public void Add(ToolBase tool)
    {
        DebugHelper.Log($"Adding Tool: {tool.GetType().Name}");
        ToBeAdded.Add(tool);
        tool.SetToolManager(this);

        _toolIds[tool.GroupId] = tool;
    }

    public void Remove(ToolBase tool)
    {
        ToBeRemoved.Add(tool);
        DebugHelper.Log($"Removing Tool: {tool.GetType().Name} : {tool.GroupId}");

        _toolIds.Remove(tool.GroupId);
    }

    public void Render()
    {
        AddTools();

        foreach (var tool in Tools)
        {
           tool.Render();
        }

        RemoveTools();
    }

    public bool ContainsGroupId(string groupId)
    {
        return _toolIds.ContainsKey(groupId);
    }

    // Private Methods

    private void AddTools()
    {
        foreach (var tool in ToBeRemoved)
        {
            Tools.Remove(tool);
        }
        ToBeRemoved.Clear();
    }

    private void RemoveTools()
    {
        foreach (var tool in ToBeAdded)
        {
            Tools.Add(tool);
        }
        ToBeAdded.Clear();
    }

    public IEnumerator<ToolBase> GetEnumerator()
    {
        return Tools.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
