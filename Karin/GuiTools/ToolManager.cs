using System.Collections;
namespace Karin.GuiTools;

public class ToolManager : IEnumerable
{
    public List<ToolBase> Tools { get; private set; }
    private List<ToolBase> ToBeRemoved = new List<ToolBase>();
    private List<ToolBase> ToBeAdded = new List<ToolBase>();

    public ToolManager()
    {
        Tools = new List<ToolBase>();
        ToBeRemoved = new List<ToolBase>();
    }

    public void Add(ToolBase tool)
    {
        DebugHelper.Log($"Adding Tool: {tool.GetType().Name}");
        ToBeAdded.Add(tool);
        tool.SetToolManager(this);
    }

    public void Remove(ToolBase tool)
    {
        ToBeRemoved.Add(tool);
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
