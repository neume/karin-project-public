using System.Reflection;
using Karin.AI.GOAP;

namespace Karin.Tests.AI.GOAP;

public static class ManagerReset
{
    public static void ClearAll()
    {
        ClearActionManager();
        ClearGoalManager();
    }

    private static void ClearActionManager()
    {
        var am = ActionManager.Instance;
        var t = typeof(ActionManager);

        GetField<Dictionary<int, ActionDef>>(t, "_actions", am).Clear();
        GetField<List<int>>(t, "_actionsList", am).Clear();
        GetField<HashSet<int>>(t, "_actionsSet", am).Clear();
    }

    private static void ClearGoalManager()
    {
        var gm = GoalManager.Instance;
        var t = typeof(GoalManager);

        GetField<Dictionary<int, GoalDef>>(t, "_goals", gm).Clear();
        GetField<List<int>>(t, "_goalsList", gm).Clear();
        GetField<HashSet<int>>(t, "_goalsSet", gm).Clear();
    }

    private static TField GetField<TField>(Type type, string name, object instance)
    {
        var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
        if (f == null)
            throw new MissingFieldException(type.FullName, name);
        return (TField)f.GetValue(instance)!;
    }
}

public class PlannerTests : IDisposable
{
    public PlannerTests()
    {
        ManagerReset.ClearAll();
    }

    public void Dispose()
    {
        ManagerReset.ClearAll();
    }

    private static GoalDef MakeGoal(int goalId, params (int key, int value)[] states)
    {
        return new GoalDef
        {
            GoalId = goalId,
            Priority = 0,
            States = states.Select(s => new State { Key = s.key, Value = s.value }).ToArray()
        };
    }

    private static ActionDef MakeAction(
        int actionId,
        int cost,
        (int key, int value)[] pre = null,
        (int key, int value)[] eff = null)
    {
        var a = new ActionDef
        {
            ActionId = actionId,
            ExecutorId = 0,
            Cost = cost
        };
        if (pre != null)
            foreach (var (k, v) in pre) a.Preconditions[k] = v;
        if (eff != null)
            foreach (var (k, v) in eff) a.Effects[k] = v;
        return a;
    }

    [Fact]
    public void TryPlanSingleGoal_ReturnsTrue_WhenSingleActionEffectsMatchGoal_AndWorldSatisfiesPreconditions()
    {
        var blackboard = new Dictionary<int, int> { [0] = 1, [1] = 0 };

        var goal = MakeGoal(goalId: 100, (1, 1));

        var action = MakeAction(actionId: 10,
                                cost: 1,
                                pre: new[] { (0, 1) },
                                eff: new[] { (1, 1) });

        ActionManager.Instance.AddAction(action);

        var ok = Planner.TryPlanSingleGoal(goal, new[] { 10 }, blackboard, out var planned);
        Assert.True(ok);
        Assert.Single(planned);
        Assert.Equal(10, planned[0]);
    }

    [Fact]
    public void TryPlanSingleGoal_ReturnsPlan_WhenSingleActionEffectsMatchGoal_AndWorldSatisfiesPreconditions()
    {
        var blackboard = new Dictionary<int, int> { [0] = 1 };

        // Goal requires K1=1
        var goal = MakeGoal(goalId: 100, (1, 1));
        GoalManager.Instance.AddGoal(goal);

        // Two actions both achieve the goal (effects K1=1), different costs; both require K0=1
        var expensive = MakeAction(actionId: 10, cost: 5,
                                   pre: new[] { (0, 1) },
                                   eff: new[] { (1, 1) });
        var cheap = MakeAction(actionId: 11, cost: 2,
                               pre: new[] { (0, 1) },
                               eff: new[] { (1, 1) });

        ActionManager.Instance.AddAction(expensive);
        ActionManager.Instance.AddAction(cheap);

        var actionIds = new[] { 10, 11 };

        // Call the single-goal API directly (bypasses Planner.Satisfies quirk)
        var ok = Planner.TryPlanSingleGoal(goal, actionIds, blackboard, out var planned);

        Assert.True(ok);
        Assert.Single(planned);
        Assert.Equal(11, planned[0]); // cheapest action chosen
    }

    [Fact]
    public void TryPlan_ReturnsFalse_WhenWorldMatchesGoal_AndActionPreconditionsAreSatisfied()
    {
        // World satisfies the precondition K0=1
        var blackboard = new Dictionary<int, int> { [0] = 1 };

        blackboard[1] = 1;

        // One goal requiring K1=1
        var goal = MakeGoal(goalId: 200, (1, 1));
        GoalManager.Instance.AddGoal(goal);

        var action = MakeAction(actionId: 20, cost: 3,
                                pre: new[] { (0, 1) },
                                eff: new[] { (1, 1) });
        ActionManager.Instance.AddAction(action);

        var goalIds = new[] { 200 };
        var actionIds = new[] { 20 };

        var ok = Planner.TryPlan(goalIds, actionIds, blackboard, out var chosenGoalId, out var planned);

        Assert.False(ok);
        Assert.Equal(-1, chosenGoalId);
    }

    [Fact]
    public void TryPlanSingleGoal_ReturnsFalse_WhenNoActionHasRequiredEffects()
    {
        var blackboard = new Dictionary<int, int> { [0] = 1 };

        // Goal wants K2=1
        var goal = MakeGoal(goalId: 300, (2, 1));
        GoalManager.Instance.AddGoal(goal);

        // Only actions changing K1, not K2 -> cannot satisfy goal's required effects
        var a1 = MakeAction(actionId: 30, cost: 1, eff: new[] { (1, 1) });
        var a2 = MakeAction(actionId: 31, cost: 1, eff: new[] { (1, 0) });

        ActionManager.Instance.AddAction(a1);
        ActionManager.Instance.AddAction(a2);

        var actionIds = new[] { 30, 31 };

        var ok = Planner.TryPlanSingleGoal(goal, actionIds, blackboard, out var planned);

        Assert.False(ok);
        Assert.Empty(planned);
    }

    [Fact]
    public void TryPlan_ReturnsPlan_ForComplexGoalWithMultipleActions()
    {
        var blackboard = new Dictionary<int, int> { [0] = 1, [1] = 0, [2] = 0 };
    
        // Complex goal requiring K1=1 and K2=1
        var goal = MakeGoal(goalId: 400, (1, 1), (2, 1));
        GoalManager.Instance.AddGoal(goal);
    
        // Multiple actions with varying preconditions and effects
        var action1 = MakeAction(actionId: 40, cost: 2,
                                 pre: new[] { (0, 1) },
                                 eff: new[] { (1, 1) }); // Achieves K1=1
        var action2 = MakeAction(actionId: 41, cost: 3,
                                 pre: new[] { (1, 1) },
                                 eff: new[] { (2, 1) }); // Achieves K2=1 but depends on K1=1
        var action3 = MakeAction(actionId: 42, cost: 8,
                                 pre: new[] { (0, 1) },
                                 eff: new[] { (2, 1) }); // Achieves K2=1 directly but more expensive
    
        ActionManager.Instance.AddAction(action1);
        ActionManager.Instance.AddAction(action2);
        ActionManager.Instance.AddAction(action3);
    
        var goalIds = new[] { 400 };
        var actionIds = new[] { 40, 41, 42 };
    
        var ok = Planner.TryPlan(goalIds, actionIds, blackboard, out var chosenGoalId, out var planned);
    
        Assert.True(ok);
        Assert.Equal(400, chosenGoalId);
        Assert.Equal(2, planned.Count());
        Assert.Contains(40, planned); // Select action1 first to achieve K1=1
        Assert.Contains(41, planned); // Then select action2 to achieve K2=1
    }
}
