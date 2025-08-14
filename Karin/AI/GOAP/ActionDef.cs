namespace Karin.AI.GOAP;

public class ActionDef
{
    public int ActionId;
    public int ExecutorId;
    public float Cost;
    public Dictionary<int, int> Preconditions;
    public Dictionary<int, int> Effects;

    public ActionDef()
    {
        Preconditions = new Dictionary<int, int>();
        Effects = new Dictionary<int, int>();
    }
}
