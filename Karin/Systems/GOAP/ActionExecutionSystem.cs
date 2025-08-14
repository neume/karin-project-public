using DefaultEcs;
using DefaultEcs.System;
using Karin.AI.GOAP;
using Karin.AI.GOAP.Components;
using Microsoft.Xna.Framework;

public class ActionExecutionSystem : AEntitySetSystem<GameTime>
{
    public ActionExecutionSystem(World world)
        : base(world.GetEntities()
                .With<AgentTagComponent>()
                .With<PlannerStateComponent>()
                .With<PlanBufferComponent>()
                .AsSet())
    {
    }

    protected override void Update(GameTime gameTime, in Entity entity)
    {
        ref var planner = ref entity.Get<PlannerStateComponent>();
        ref var planBuffer = ref entity.Get<PlanBufferComponent>();

        if (planner.Status != PlanStatus.Planned && planner.Status != PlanStatus.Executing)
            return;

        if(planBuffer.Index >= planBuffer.Length)
        {
            planner.Status = PlanStatus.Completed;
            return;
        }

        var actionId = planBuffer.ActionIds[planBuffer.Index];

        if(!entity.Has<CurrentActionComponent>() || entity.Get<CurrentActionComponent>().ActionId != actionId)
        {
            if(entity.Has<CurrentActionComponent>())
            {
                var currentActionId = entity.Get<CurrentActionComponent>().ActionId;
                var prevExec = GetExecutor(currentActionId);
                prevExec.Stop(entity, currentActionId, entity.World);
            }

            var exec = GetExecutor(actionId);
            entity.Set(
                new CurrentActionComponent
                {
                    ActionId = actionId,
                    Started = true,
                    Complete = false
                }
            );

            exec.Start(entity, actionId, entity.World);
            planner.Status = PlanStatus.Executing;
        }
        else
        {
            var current = entity.Get<CurrentActionComponent>();
            var exec = GetExecutor(actionId);
            bool complete = current.Complete;
            bool valid = true;

            exec.Update(entity, actionId, entity.World, gameTime, ref current.Complete, ref valid);

            if(!valid) { entity.Set<NeedsReplanComponent>(); return; }

            if (complete)
            {
                exec.Stop(entity, actionId, entity.World);
                planBuffer.Index++;
                entity.Remove<CurrentActionComponent>();
            }
            else
            {
                current.Complete = false;
                entity.Set(current);
            }
        }
    }

    static IActionExecutor GetExecutor(int actionId)
    {
        var actionDef = ActionManager.Instance.GetAction(actionId);

        if (actionDef == null)
            throw new Exception("No action definition found");

        return ActionExecutors.Get(actionDef.ExecutorId);
    }
}
