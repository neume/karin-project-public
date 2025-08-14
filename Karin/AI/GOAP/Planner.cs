namespace Karin.AI.GOAP;

public static class Planner
{
    class Node
    {
        public ActionDef? Action;
        public Dictionary<int, int> RequiredStates;
        public List<Node> Children = new List<Node>();
        public Node? Parent;
        public float Cost;

        public Node(Node? parent, float cost, ActionDef? action, Dictionary<int, int> requiredStates)
        {
            this.Parent = parent;
            this.Action = action;
            if (parent != null)
            {
                this.Cost = parent.Cost + cost;
            }
            else
            {
                this.Cost = cost;
            }

            this.RequiredStates = requiredStates;
        }
    }

    public static bool TryPlan(
            int[] goalIds,
            int[] actionIds,
            Dictionary<int, int> blackboard,
            out int chosenGoalId,
            out int[] plannedActionIds)
    {
        chosenGoalId = -1;
        plannedActionIds = Array.Empty<int>();

        goalIds = SortGoals(goalIds);

        foreach(var goalId in goalIds)
        {
            var goalDef = GoalManager.Instance.GetGoal(goalId);

            var need = ConvertToDictionary(goalDef.States);
            if (!IsSatisfied(need, blackboard))
            {
                if (TryPlanSingleGoal(goalDef, actionIds, blackboard, out var plan))
                {
                    chosenGoalId = goalDef.GoalId;
                    plannedActionIds = plan;
                    return true;
                }
            }
        }

        return false;
    }

    public static bool TryPlanSingleGoal(GoalDef goalDef, int[] actionIds, Dictionary<int, int> blackboard, out int[] plannedActionIds)
    {

        plannedActionIds = Array.Empty<int>();

        var desiredStates = ConvertToDictionary(goalDef.States);

        if (IsSatisfied(desiredStates, blackboard))
            return true;

        var leaves = new List<Node>();
        var root = new Node(parent: null, cost: 0, action: null, requiredStates: CloneDict(desiredStates));

        // Filter to only known actions
        var usable = ActionManager.Instance.GetActions(actionIds);
        var visited = new HashSet<int>();

        BuildGraph(root, leaves, usable, blackboard, visited);
        if (leaves.Count == 0) return false;

        var cheapest = GetCheapestLeaf(leaves);
        plannedActionIds = GetPlan(cheapest!);
        return true;
    }

    private static int[] SortGoals(int[] goalIds)
    {

        return goalIds
            .OrderByDescending(goalId => GoalManager.Instance.GetGoal(goalId).Priority)
            .ToArray();
    }

    private static bool IsSatisfied(Dictionary<int, int> need, Dictionary<int, int> world)
    {
        foreach (var kv in need)
            if (!world.TryGetValue(kv.Key, out var v) || v != kv.Value)
                return false;
        return true;
    }

    private static bool BuildGraph(
            Node parent,
            List<Node> leaves,
            int[] usableActions,
            Dictionary<int, int> blackboard,
            HashSet<int> visited)
    {
        bool expanded = false;

        foreach (int actionId in usableActions)
        {
            var action = ActionManager.Instance.GetAction(actionId);

            var achieved = Intersect(parent.RequiredStates, action.Effects);
            if (achieved.Count == 0)
                continue;

            var nextRequired = Regress(parent.RequiredStates, action.Effects, action.Preconditions);

            if (IsSatisfied(nextRequired, blackboard))
            {
                var child = new Node(
                        parent: parent,
                        cost: action.Cost,
                        action: action,
                        requiredStates: CloneDict(nextRequired));
                parent.Children.Add(child);
                leaves.Add(child);
                expanded = true;
                continue;
            }

            // Prune
            var sig = Signature(nextRequired, usableActions, actionId);
            if (!visited.Add(sig))
                continue;

            // var subset = RemoveOne(usableActions, actionId);

            var childNode = new Node(
                    parent: parent,
                    cost: action.Cost,
                    action: action,
                    requiredStates: CloneDict(nextRequired));
            parent.Children.Add(childNode);

            // if (BuildGraph(childNode, leaves, subset, blackboard, visited))
            if (BuildGraph(childNode, leaves, usableActions, blackboard, visited))
                expanded = true;
        }

        return expanded;
    }

    private static Node? GetCheapestLeaf(List<Node> leaves)
    {
        Node? cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else if (leaf.Cost < cheapest.Cost)
                cheapest = leaf;
        }

        return cheapest;
    }

    private static int[] GetPlan(Node leaf)
    {
        var plan = new List<int>();
        Node? node = leaf;

        while (node != null && node.Action != null)
        {
            plan.Add(node.Action.ActionId);
            node = node.Parent;
            if (node == null)
                break;
        }

        plan.Reverse();

        return plan.ToArray();
    }

    private static Dictionary<int, int> ConvertToDictionary(State[] states)
    {
        var result = new Dictionary<int, int>(states.Length);
        foreach (var state in states)
            result[state.Key] = state.Value;

        return result;
    }

    // PERF: Refactor this Planner for performance. Do not use cloned dictionaries.
    private static Dictionary<int, int> CloneDict(Dictionary<int, int> dictionary)
    {
        var clone = new Dictionary<int, int>(dictionary.Count);
        foreach (var pair in dictionary) clone[pair.Key] = pair.Value;
        return clone;
    }

    // private static int[] RemoveOne(int[] array, int value)
    // {
    //     var result = new int[array.Length - 1];
    //     int counter = 0;
    //     for (int i = 0; i < array.Length; i++)
    //         if (array[i] != value) result[counter++] = array[i];
    //     return result;
    // }

    // From chatgpt
    private static int Signature(Dictionary<int, int> need, int[] actions, int removedAction)
    {
        unchecked
        {
            int h = 17;
            foreach (var kv in need)
            {
                h = h * 31 + kv.Key.GetHashCode();
                h = h * 31 + kv.Value.GetHashCode();
            }
            h = h * 31 + (actions.Length - 1);
            h = h * 31 + removedAction.GetHashCode();
            return h;
        }
    }

    private static Dictionary<int, int> Intersect(Dictionary<int, int> required, Dictionary<int, int> effects)
    {
        var result = new Dictionary<int, int>();
        foreach (var effect in effects)
            if (required.TryGetValue(effect.Key, out var v) && v == effect.Value)
                result[effect.Key] = effect.Value;
        return result;
    }

    private static Dictionary<int, int> Regress(
            Dictionary<int, int> required,
            Dictionary<int, int> effects,
            Dictionary<int, int> preconditions)
    {
        // next = required − effects
        var next = new Dictionary<int, int>(required.Count + preconditions.Count);
        foreach (var pair in required)
            if (!effects.TryGetValue(pair.Key, out var effectValue) || effectValue != pair.Value)
                next[pair.Key] = pair.Value;

        // union with preconditions
        foreach (var precondition in preconditions)
            next[precondition.Key] = precondition.Value;

        return next;
    }
}
