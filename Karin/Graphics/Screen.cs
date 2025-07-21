using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karin.Graphics;

public class Screen : IDisposable
{
  private readonly static int MinDim = 64;
  private readonly static int MaxDim = 1024;

  private bool _isDisposed;
  private Game _game;
  private RenderTarget2D _target;

  public int Width => _target.Width;
  public int Height => _target.Height;


  public Screen(Game game, int width, int height)
  {
    width = Util.Clamp(width, Screen.MinDim, Screen.MaxDim);
    height = Util.Clamp(height, Screen.MinDim, Screen.MaxDim);

    _game = game;
    _isDisposed = false;

    _target = new RenderTarget2D(_game.GraphicsDevice, width, height);
  }

  public void Dispose()
  {
    if (_isDisposed)
      return;

    _target?.Dispose();
    _isDisposed = true;
  }

  public void Set()
  {
    _game.GraphicsDevice.SetRenderTarget(_target);
  }

  public void Unset()
  {
    _game.GraphicsDevice.SetRenderTarget(null);
  }

  public void Present(Renderer renderer)
  {
    Rectangle destinationRectangle = CalculateDestinationRectangle();

    renderer.Begin(null);
    renderer.Draw(_target, null, destinationRectangle, Color.White);
    renderer.End();
  }

  private Rectangle CalculateDestinationRectangle()
  {
    Rectangle backbufferBounds  = _game.GraphicsDevice.PresentationParameters.Bounds;
    float backbufferAspectRatio = (float)backbufferBounds.Width / backbufferBounds.Height;
    float screenAspectRatio = (float)Width/Height;

    float rx = 0f;
    float ry = 0f;
    float rw = backbufferBounds.Width;
    float rh = backbufferBounds.Height;

    if(backbufferAspectRatio > screenAspectRatio)
    {
      rw = rh * screenAspectRatio;
      rx = ((float)backbufferBounds.Width - rw) / 2;
    }
    else
    {
      rh = rw / screenAspectRatio;
      ry = ((float)backbufferBounds.Height - rh) / 2;
    }

    Rectangle result = new Rectangle((int)rx, (int)ry, (int)rw, (int)rh);
    return result;
  }
}
