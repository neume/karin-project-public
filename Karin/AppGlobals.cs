using Karin.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

public static class AppGlobals
{
    public static Renderer Renderer;
    public static GraphicsDevice GraphicsDevice;
    public static Game Game;
    public static Camera Camera;
    public static Screen Screen;
    public static List<Type> InspectableTypes = new List<Type>();

    public static List<Type> GetInspectableTypes()
    {

        if (InspectableTypes.Count > 0)
            return InspectableTypes;

        InspectableTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsValueType || t.IsClass) // structs or classes
            .Where(t => t.GetCustomAttribute<ToolInspectableAttribute>() != null)
            .ToList();

        return InspectableTypes;

    }

}
