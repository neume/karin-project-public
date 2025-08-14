namespace Karin.AI.GOAP;

// WARN: This is a singleton. Do not use it in a multi-threaded environment.
public class GoalManager
{
    public static GoalManager Instance { get; } = new();

    private readonly Dictionary<int, GoalDef> _goals;
    private readonly List<int> _goalsList;
    private readonly HashSet<int> _goalsSet;

    public GoalManager()
    {
        _goals = new Dictionary<int, GoalDef>();
        _goalsList = new List<int>();
        _goalsSet = new HashSet<int>();
    }

    public void AddGoal(GoalDef goal)
    {
        _goals[goal.GoalId] = goal;
        if (_goalsSet.Add(goal.GoalId))
            _goalsList.Add(goal.GoalId);
    }

    public void RemoveGoal(int goalId)
    {
        _goals.Remove(goalId);
        _goalsList.Remove(goalId);
        _goalsSet.Remove(goalId);
    }

    public bool ContainsGoal(int goalId)
        => _goalsSet.Contains(goalId);

    public bool TryGetGoal(int goalId, out GoalDef goal)
        => _goals.TryGetValue(goalId, out goal);

    public GoalDef GetGoal(int goalId)
        => _goals[goalId];

    public int[] ToArray()
        => _goalsList.ToArray();

    public int[] GetGoals(int[] goalIds)
    {
        var result = new int[goalIds.Length];
        int count = 0;

        foreach (var goalId in goalIds)
            if (ContainsGoal(goalId))
                result[count++] = goalId;

        if (count != result.Length)
            Array.Resize(ref result, count);
        
        return result;
    }
}
