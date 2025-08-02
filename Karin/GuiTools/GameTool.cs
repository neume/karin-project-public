using ImGuiNET;
using System.Numerics;

namespace Karin.GuiTools;

public class GameTool : ToolBase
{
    public GameTool(bool active = false) : base(active) {}


    public override void Draw()
    {
        ImGui.Begin("Game Stats", ref Active);
        var text = "FPS: " + Application.Instance.GameStats.FPS;
        ImGui.TextColored(new Vector4(1, 1, 0, 1), text);
        text = "UPS: " + Application.Instance.GameStats.UPS;
        ImGui.TextColored(new Vector4(1, 1, 0, 1), text);
        text = "FUPS: " + Application.Instance.GameStats.FUPS;
        ImGui.TextColored(new Vector4(1, 1, 0, 1), text);
        text = "Elapsed Time: " + Application.Instance.GameStats.TotalElapsedTimeFormatted();
        ImGui.TextColored(new Vector4(1, 1, 0, 1), text);
        ImGui.End();
    }
}
