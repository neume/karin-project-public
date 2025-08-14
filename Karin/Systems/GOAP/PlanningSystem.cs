using DefaultEcs;
using DefaultEcs.System;
using Karin.AI.GOAP;
using Karin.AI.GOAP.Components;
using Microsoft.Xna.Framework;

public class PlanningSystem : AEntitySetSystem<GameTime>
{
    public PlanningSystem(World world)
        : base(world.GetEntities()
                .With<AgentTagComponent>()
                .With<AgentBlackboardComponent>()
                .AsSet())
    {
    }

    protected override void Update(GameTime gameTime, in Entity entity)
    {
        ref var planner = ref entity.Get<PlannerStateComponent>();
        if (entity.Has<NeedsReplanComponent>() || planner.Status == PlanStatus.None)
        {
            ref var blackboard = ref entity.Get<AgentBlackboardComponent>();
            ref var goal = ref entity.Get<AgentGoalsComponent>();
            ref var actions = ref entity.Get<AgentActionsComponent>();

            int[] goalIds = GoalManager.Instance.GetGoals(goal.GoalIds);
            int[] agentActionIds = ActionManager.Instance.GetActions(actions.ActionIds);

            if(Planner.TryPlan(
                        goalIds,
                        agentActionIds,
                        blackboard.Values,
                        out var chosenGoalId,
                        out var plannedActionIds))
            {
                entity.Set(new PlanBufferComponent { ActionIds = plannedActionIds, Length = plannedActionIds.Length, Index = 0 });
                planner.CurrentGoalId = chosenGoalId;
                planner.Status = PlanStatus.Planned;
                entity.Remove<NeedsReplanComponent>();
            }
            else
            {
                planner.Status = PlanStatus.None;

                if (entity.Has<CurrentActionComponent>())
                    entity.Remove<CurrentActionComponent>();
                if (entity.Has<PlanBufferComponent>())
                    entity.Remove<PlanBufferComponent>();
            }
        }
    }
}
