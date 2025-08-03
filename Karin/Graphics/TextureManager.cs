using Microsoft.Xna.Framework.Graphics;

namespace Karin.Graphics;

public class TextureManager
{
    private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    private static Dictionary<string, string> texturePaths = new Dictionary<string, string>();


    public static Texture2D? GetTexture(string name)
    {
        if (textures.ContainsKey(name))
            return textures[name];
        else if (texturePaths.ContainsKey(name))
        {
            LoadTexture(name, texturePaths[name]);
            return textures[name];
        }
        else
            return null;
    }

    public static void LoadTexture(string name, string path)
    {
        textures[name] = ContentLoader.LoadTexture(path);
        texturePaths[name] = path;
    }

    public static void AddTexture(string name, string path)
    {
        texturePaths[name] = path;
    }
}
