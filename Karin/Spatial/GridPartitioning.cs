using DefaultEcs;

namespace Karin.Spatials;

public class GridPartitioning
{
  float _cellSize;
  public Dictionary < (int, int), List < Point >> Grid = new();
  public int MinX, MinY, MaxX, MaxY;
  public int Count { get; private set; }

  public GridPartitioning(float cellSize)
  {
    _cellSize = cellSize;
    MinX = MinY = int.MaxValue;
    MaxX = MaxY = int.MinValue;
    Grid = new Dictionary < (int, int), List < Point >> ();
    Count = 0;
  }

  public (int, int) GetCell(Point point)
  {
    return ((int)(point.X / _cellSize), (int)(point.Y / _cellSize));
  }

  public (int, int) GetCell(float x, float y)
  {
    return ((int)(x / _cellSize), (int)(y / _cellSize));
  }

  public Point Add(float x, float y, Entity entity)
  {
       var point = new Point(x, y, entity);
       Add(point);
       return point;
  }


  public void Add(Point point)
  {
    var cell = GetCell(point);
    if (!Grid.ContainsKey(cell))
      Grid[cell] = new List < Point > ();

    Grid[cell].Add(point);
    if (cell.Item1 < MinX) MinX = cell.Item1;
    if (cell.Item2 < MinY) MinY = cell.Item2;
    if (cell.Item1 > MaxX) MaxX = cell.Item1;
    if (cell.Item2 > MaxY) MaxY = cell.Item2;
    Count++;
  }

  public void Remove(Point point)
  {
    var cell = GetCell(point);
    if (Grid.ContainsKey(cell))
      Grid[cell].Remove(point);
  }

  public List<Point> Query(float x, float y, float range, List<Point>? nearbyPoints = null)
  {
    var minCell = GetCell(x - range, y - range);
    var maxCell = GetCell(x + range, y + range);

    if(nearbyPoints == null)
      nearbyPoints = new List<Point>();

    for (int i = minCell.Item1; i <= maxCell.Item1; i++)
    {
      for (int j = minCell.Item2; j <= maxCell.Item2; j++)
      {
        if (Grid.ContainsKey((i, j)))
        {
          foreach(var point in Grid[(i, j)])
          {
            nearbyPoints.Add(point);
          }
        }
      }
    }
    return nearbyPoints;
  }

  public void Clear()
  {
    Grid.Clear();
    Count = 0;
  }
}
