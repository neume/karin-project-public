namespace Karin.AI.GOAP;

public static class StateIds
{
    private static readonly Dictionary<string,int> _toId = new();
    private static readonly List<string> _names = new();
    public static int GetId(string name)
    {
        if (_toId.TryGetValue(name, out var id)) return id;
        id = _names.Count;
        _toId[name] = id;
        _names.Add(name);
        return id;
    }
    public static string GetName(int id) => _names[id];
}
