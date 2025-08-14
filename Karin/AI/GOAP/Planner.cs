namespace Karin.AI.GOAP;

public static class Planner
{
    class Node
    {
        public ActionDef? Action;
        public Dictionary<int, int> RequiredState;
        public List<Node> Children = new List<Node>();
        public Node? Parent;
        public float Cost;

        public Node(Node? parent, float cost, ActionDef? action, Dictionary<int, int> states)
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

            this.RequiredState = states;
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

        foreach(var goalId in goalIds)
        {
            var goalDef = GoalManager.Instance.GetGoal(goalId);

            if (Satisfies(goalDef, blackboard))
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

        List<Node> leaves = new List<Node>();

        var desiredState =  ConvertToDictionary(goalDef.States);

        Node root = new Node(parent: null, cost: 0, action: null, states: desiredState);

        BuildGraph(root, leaves, actionIds, blackboard);

        if (leaves.Count == 0)
            return false;

        Node? cheapest = GetCheapestLeaf(leaves);

        if (cheapest == null)
            return false;

        plannedActionIds = GetPlan(cheapest);

        return true;
    }

    private static bool Satisfies(GoalDef goalDef, Dictionary<int, int> blackboard)
    {
        foreach (var state in goalDef.States)
            if (!blackboard.ContainsKey(state.Key) || blackboard[state.Key] != state.Value)
                return false;

        return true;
    }

    private static bool BuildGraph(Node parent, List<Node> leaves, int[] usableActions, Dictionary<int, int> worldboard)
    {
        foreach (int actionId in usableActions)
        {
            var action = ActionManager.Instance.GetAction(actionId);
            if(!HasAllEffects(action, parent.RequiredState))
                continue;

            Node child = new Node(
                    parent,
                    action.Cost,
                    action,
                    action.Preconditions);
            parent.Children.Add(child);

            if (GoalAchieved(child.RequiredState, worldboard))
                leaves.Add(child);
            else
            {
                int[] subset = new int[usableActions.Length];

                for(int i = 0; i < usableActions.Length; i++)
                    if (actionId != action.ActionId)
                        subset[i] = usableActions[i];
                
                BuildGraph(child, leaves, subset, worldboard);
            }
        }

        if (parent.Children.Count == 0)
            return false;

        return true;
    }

    private static bool HasAllEffects(ActionDef action, Dictionary<int, int> state)
    {
        foreach (KeyValuePair<int, int> effect in action.Effects)
            if (!state.ContainsKey(effect.Key) || state[effect.Key] != effect.Value)
                return false;

        return true;
    }

    private static bool GoalAchieved(Dictionary<int, int> goal, Dictionary<int, int> states)
    {
        foreach (KeyValuePair<int, int> pair in goal)
            if (!states.ContainsKey(pair.Key) || states[pair.Key] != pair.Value)
                return false;

        return true;
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

        return plan.ToArray();
    }

    private static Dictionary<int, int> ConvertToDictionary(State[] states)
    {
        var result = new Dictionary<int, int>(states.Length);
        foreach (var state in states)
            result[state.Key] = state.Value;

        return result;
    }
}
