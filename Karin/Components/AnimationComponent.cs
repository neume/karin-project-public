using Karin.GuiTools;

namespace Karin.Components;

[ToolInspectable]
public struct AnimationComponent
{
    [ToolSerializeField("AnimationClipName")]
    public string AnimationClipName;

    [ToolSerializeField("FrameIndex")]
    public int FrameIndex;

    [ToolSerializeField("NextFrameUntil")]
    public float NextFrameUntil;
}
