using Aether = nkast.Aether.Physics2D.Dynamics;

namespace Karin.Physics;

public class PhysicsBodyManager
{
    private Dictionary<int, Aether.Body> _bodyMap = new();
    private Aether.World _physicsWorld;
    private int _bodyCount = 0;

    public PhysicsBodyManager(Aether.World physicsWorld)
    {
        _physicsWorld = physicsWorld;
    }

    public int AddBody(Aether.Body body)
    {
        _bodyMap.Add(_bodyCount, body);
        _bodyCount++;

        return _bodyCount - 1;
    }

    public void RemoveBody(int id)
    {
        _bodyMap.Remove(id);
    }

    public Aether.Body? GetBody(int id)
    {
        if (_bodyMap.ContainsKey(id))
            return _bodyMap[id];
        return null;
    }
}
