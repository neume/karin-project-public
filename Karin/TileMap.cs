using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karin;

public class TileMap
{
    public Dictionary<Vector2, int> Map;
    public Dictionary<int, Rectangle> TextureStore;
    public Texture2D TextureAtlas;
    public Vector2 Position;

    private int _width;
    private int _height;

    public TileMap(string path, Texture2D textureAtlas, int width, int height, Vector2 position)
    {
        Map = LoadTileMap(path);
        TextureStore = new Dictionary<int, Rectangle>();
        TextureAtlas = textureAtlas;
        _width = width;
        _height = height;
    }

    private Dictionary<Vector2, int> LoadTileMap(string path)
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

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var item in Map)
        {
            Rectangle dest = new Rectangle(
                ((int)item.Key.X * _width),
                ((int)item.Key.Y * _height),
                _width,
                _height
            );
            Rectangle src = GetTileRectangle(item.Value);
            spriteBatch.Draw(TextureAtlas, dest, src, Color.White);
        }
    }

    private Rectangle GetTileRectangle(int index)
    {
        if(TextureStore.ContainsKey(index))
            return TextureStore[index];

        var rectangle = CalculateRectangle(index);
        TextureStore.Add(index, rectangle);
        return rectangle;
    }

    private Rectangle CalculateRectangle(int index)
    {
        int colCount = TextureAtlas.Width / _width;
        int rowCount = TextureAtlas.Height / _height;

        int col = index % colCount;
        int row = index / colCount;

        return new Rectangle(col * _width, row * _height, _width, _height);
    }
}
