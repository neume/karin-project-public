using Microsoft.Xna.Framework;

namespace Karin.Graphics;

public class Camera
{
  public readonly static float MinZ = 1f;
  public readonly static float MaxZ = 1000f;

  public readonly static float MinZoom = 1f;
  public readonly static float MaxZoom = 20f;

  public Vector2 Position;
  public float Z { get; private set; }
  public float BaseZ { get; private set; }

  public float AspectRatio { get; private set; }
  public float FieldOfView { get; private set; }

  public Matrix View => _view;
  public Matrix Projection => _projection;

  public float Zoom = 1;

  private Matrix _view;
  private Matrix _projection;

  public Camera(Screen screen)
  {
    AspectRatio = (float)screen.Width / (float)screen.Height;
    FieldOfView = MathHelper.PiOver2;

    Position = new Vector2(0, 0);
    BaseZ = GetZFromHeight(screen.Height);
    Z = BaseZ;
  }

  public Camera(float Width, float Height)
  {
    AspectRatio = Width / Height;
    FieldOfView = MathHelper.PiOver2;

    Position = new Vector2(0, 0);
    BaseZ = GetZFromHeight(Height);
    Z = BaseZ;
  }

  public Matrix TranslationMatrix => Matrix.CreateTranslation(-Position.X, -Position.Y, 0);

  public void UpdateMatrices()
  {
    _view = Matrix.CreateLookAt(new Vector3(0, 0, Z), Vector3.Zero, Vector3.Up);
    _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, MinZ, MaxZ);
  }

  public float GetZFromHeight(float height)
  {
    return (0.5f * height) / (float)Math.Tan(0.5f * FieldOfView);
  }

  public float GetHeightFromZ()
  {
    return Z * MathF.Tan(0.5f * FieldOfView) * 2f;
  }

  public void MoveZ(float amount)
  {
    Console.WriteLine($"Z: {Z}");
    Z += amount;
    Z = Util.Clamp(Z, MinZ, MaxZ);
  }

  public void ResetZ()
  {
    Z = BaseZ;
  }

  public void Move(Vector2 amount)
  {
    Position += amount;
  }

  public void MoveTo(Vector2 position)
  {
    Position = position;
  }

  public void IncZoom(float amount)
  {
    Zoom += amount;
    Zoom = Util.Clamp(Zoom, MinZoom, MaxZoom);
    Z = BaseZ / Zoom;
  }

  public void DecZoom(float amount)
  {
    Zoom -= amount;
    Zoom = Util.Clamp(Zoom, MinZoom, MaxZoom);
    Z = BaseZ / Zoom;
  }

  public void SetZoom(float zoom)
  {
    Zoom = zoom;
    Zoom = Util.Clamp(Zoom, MinZoom, MaxZoom);
    Z = BaseZ / Zoom;
  }

  public void GetExtents(out float width, out float height)
  {
    height = GetHeightFromZ();
    width = height * AspectRatio;
  }

  public void GetExtents(out float left, out float right, out float bottom, out float top)
  {
    GetExtents(out float width, out float height);

    left = Position.X - width * 0.5f;
    right = left + width;
    bottom = Position.Y - height * 0.5f;
    top = bottom + height;
  }

  public void GetExtents(out Vector2 min, out Vector2 max)
  {
    GetExtents(out float left, out float right, out float bottom, out float top);

    min = new Vector2(left, bottom);
    max = new Vector2(right, top);
  }

  public Vector2 ScreenToWorld(Vector2 screenPosition)
  {
    return Vector2.Transform(screenPosition, Matrix.Invert(_view * _projection));
  }
}
