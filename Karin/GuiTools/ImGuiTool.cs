namespace Karin.GuiTools;

public class ImGuiTool
{
    public bool Active = false;

    public ImGuiTool(bool active = false) => Active = active;

    public virtual void Render()
    {
    }

    public void SetActive(bool active) => Active = active;
}
