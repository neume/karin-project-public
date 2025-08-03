namespace Karin.GuiTools;

[AttributeUsage(AttributeTargets.Field)]
public class ToolSerializeFieldAttribute : Attribute
{
    public string? Label { get; }
    public bool Editable { get; }

    public ToolSerializeFieldAttribute(string? label = null, bool editable = true)
    {
        Label = label;
        Editable = editable;
    }
}
