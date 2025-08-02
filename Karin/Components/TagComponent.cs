using Karin.GuiTools;

namespace Karin.Components;

[ToolInspectable]
public struct TagComponent
{
    [ToolSerializeField("Tag")]
    public string Name;
}
