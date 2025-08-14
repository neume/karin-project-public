namespace Karin.AI.GOAP.Components;

public enum PlanStatus : byte { None, Planned, Executing, Completed }

public struct PlannerStateComponent
{
    public int CurrentGoalId;
    public PlanStatus Status;
}

