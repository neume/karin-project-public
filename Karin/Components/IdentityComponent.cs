using Karin.GuiTools;

namespace Karin.Components;

[ToolInspectable]
public struct IdentityComponent
{
    [ToolSerializeField("String ID", false)]
    public string Id;

    [ToolSerializeField("Int ID", false)]
    public int IntId;
}
