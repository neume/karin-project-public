using Microsoft.Xna.Framework.Graphics;

public static class ContentLoader
{
    public static Texture2D LoadTexture(string path)
    {
        Texture2D texture;

        using (Stream stream = File.OpenRead(path))
        {
            texture = Texture2D.FromStream(AppGlobals.GraphicsDevice, stream);
        }

        return texture;
    }
}
