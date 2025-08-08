using ECS = DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Aether = nkast.Aether.Physics2D.Dynamics;
using Karin.Physics;
using Karin.Components;

namespace Karin.Systems;

public class PhysicsSyncSystem : AEntitySetSystem<GameTime>
{
    private PhysicsBodyManager _physicsBodyManager;
    private Aether.World _physicsWorld;

    public PhysicsSyncSystem(
            ECS.World world,
            Aether.World physicsWorld,
            PhysicsBodyManager physicsBodyManager)
        : base(world.GetEntities().With<PhysicsBodyComponent>().AsSet())
    {
        _physicsWorld = physicsWorld;
        _physicsBodyManager = physicsBodyManager;
    }

    protected override void Update(GameTime gameTime, in ECS.Entity entity)
    {
        ref var physicsBodyComponent = ref entity.Get<PhysicsBodyComponent>();
        ref var transformComponent = ref entity.Get<TransformComponent>();

        if (physicsBodyComponent.ReferenceBody == null)
            return;

        var body = _physicsBodyManager.GetBody(physicsBodyComponent.ReferenceBody.Value);

        if (body == null)
            return;

        transformComponent.Position = body.Position;
    }
}
