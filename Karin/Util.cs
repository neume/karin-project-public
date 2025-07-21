using Microsoft.Xna.Framework;

namespace Karin;

public class Util
{
    public static void ToggleFullScreen(GraphicsDeviceManager graphicsDeviceManager)
    {
        graphicsDeviceManager.HardwareModeSwitch = false;
        graphicsDeviceManager.ToggleFullScreen();
    }

    public static int Clamp(int value, int min, int max)
    {
        return Math.Max(min, Math.Min(value, max));
    }

    public static float Clamp(float value, float min, float max)
    {
        return Math.Max(min, Math.Min(value, max));
    }

    public static void Normalize(ref float x, ref float y)
    {
        float invLength = 1f / MathF.Sqrt(x * x + y * y);
        x *= invLength;
        y *= invLength;
    }

    public static Dictionary<Vector2, int> LoadTileMap(string path)
    {
        Dictionary<Vector2, int> result = new();
        StreamReader reader = new(path);
        int y = 0;
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] items = line.Split(',');

            for (int x = 0; x < items.Length; x++)
            {
                if (int.TryParse(items[x], out int value))
                {
                    result[new Vector2(x, y)] = value;
                }
            }
            y++;
        }
        return result;
    }
}
