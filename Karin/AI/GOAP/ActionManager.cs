namespace Karin.AI.GOAP;

// WARN: This is a singleton. Do not use it in a multi-threaded environment.
public class ActionManager
{
    public static ActionManager Instance { get; } = new();

    private readonly Dictionary<int, ActionDef> _actions;
    private readonly List<int> _actionsList;
    private readonly HashSet<int> _actionsSet;

    public ActionManager()
    {
        _actions = new Dictionary<int, ActionDef>();
        _actionsList = new List<int>();
        _actionsSet = new HashSet<int>();
    }

    public void AddAction(ActionDef action)
    {
        _actions[action.ActionId] = action;
        if (_actionsSet.Add(action.ActionId))
            _actionsList.Add(action.ActionId);
    }

    public void RemoveAction(int actionId)
    {
        _actions.Remove(actionId);
        _actionsList.Remove(actionId);
        _actionsSet.Remove(actionId);
    }

    public bool ContainsAction(int actionId)
        => _actionsSet.Contains(actionId);

    public bool TryGetAction(int actionId, out ActionDef action)
        => _actions.TryGetValue(actionId, out action);

    public ActionDef GetAction(int actionId)
        => _actions[actionId];

    public int[] ToArray()
        => _actionsList.ToArray();

    public int[] GetActions(int[] actionIds)
    {
        var result = new int[actionIds.Length];
        int count = 0;

        foreach (var actionId in actionIds)
            if (ContainsAction(actionId))
                result[count++] = actionId;

        if (count != result.Length)
            Array.Resize(ref result, count);

        return result;
    }
}
