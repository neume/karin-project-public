using Karin.GuiTools;

namespace Karin.Components;

[ToolInspectable]
public struct DrawInfoComponent
{
    [ToolSerializeField("Visible")]
    public bool IsVisible;

    [ToolSerializeField("ZIndex")]
    public float ZIndex;

    public DrawInfoComponent()
    {
        IsVisible = true;
        ZIndex = 0;
    }
}
