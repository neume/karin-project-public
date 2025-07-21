namespace Karin.Components;

public struct DrawInfoComponent
{
    public bool IsVisible;
    public float ZIndex;

    public DrawInfoComponent()
    {
        IsVisible = true;
        ZIndex = 0;
    }
}
