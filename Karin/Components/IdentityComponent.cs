using Karin.GuiTools;

namespace Karin.Components;

[ToolInspectable]
public struct IdentityComponent
{
    [ToolSerializeField("Visible", false)]
    public string Id;
}
