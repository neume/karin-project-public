using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karin.Graphics;

public class Renderer : IDisposable
{
    public SpriteBatch SpriteBatch;

    private bool _isDisposed;
    private Game _game;
    private BasicEffect _effect;

    public Renderer(Game game)
    {
        _game = game;
        _isDisposed = false;

        SpriteBatch = new SpriteBatch(_game.GraphicsDevice);

        _effect = new BasicEffect(_game.GraphicsDevice);
        _effect.FogEnabled = false;
        _effect.TextureEnabled = true;
        _effect.LightingEnabled = false;
        _effect.VertexColorEnabled = true;
        _effect.Texture = null;
        _effect.World = Matrix.Identity;
        _effect.View = Matrix.Identity;
        _effect.Projection = Matrix.Identity;
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        SpriteBatch?.Dispose();
        _effect?.Dispose();
        _isDisposed = true;
    }

    public void Begin()
    {
        Viewport viewport = _game.GraphicsDevice.Viewport;
        _effect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, 0, viewport.Height, 0f, 1f);
        _effect.View = Matrix.Identity;

        SpriteBatch.Begin(
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                effect: _effect,
                rasterizerState: RasterizerState.CullNone);
    }

    public void End()
    {
        SpriteBatch.End();
    }

    public void Draw(Texture2D texture, Vector2 origin, Vector2 position, Color color)
    {
        SpriteBatch.Draw(texture, position, null, color, 0f, origin, 1f, SpriteEffects.FlipVertically, 0f);
    }

    public void Draw(Texture2D texture, Rectangle? sourceRectangle, Rectangle destinationRectangle, Color color)
    {
        SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
    }

    public void Draw(Texture2D texture, Rectangle sourceRectangle, Vector2 position, Color color)
    {
        SpriteBatch.Draw(texture, position, sourceRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 0f);
    }

    public void Draw(Texture2D texture, Vector2 position)
    {
        var color = Color.White;
        SpriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 0f);
    }

}
