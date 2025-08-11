using DefaultEcs;

namespace Karin.Spatials;

public struct Point
{
  public float X { get; set; }
  public float Y { get; set; }
  public Entity Entity { get; set; }

  public Point(float x, float y, Entity entity)
  {
    X = x;
    Y = y;
    Entity = entity;
  }
}
