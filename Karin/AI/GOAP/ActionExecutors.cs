namespace Karin.AI.GOAP;

public static class ActionExecutors
{
    private static readonly Dictionary<int, IActionExecutor> _map = new();
    public static void Register(int id, IActionExecutor exec) => _map[id] = exec;
    public static IActionExecutor Get(int id) => _map[id];
}
