namespace Karin.GuiTools;

[AttributeUsage(AttributeTargets.Field)]
public class ToolSerializeFieldAttribute : Attribute
{
    public string? Label { get; }

    public ToolSerializeFieldAttribute(string? label = null)
    {
        Label = label;
    }
}
