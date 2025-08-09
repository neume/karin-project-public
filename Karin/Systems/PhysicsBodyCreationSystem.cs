using ECS = DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Aether = nkast.Aether.Physics2D.Dynamics;
using Karin.Physics;
using Karin.Components;

namespace Karin.Systems;

public class PhysicsBodyCreationSystem : AEntitySetSystem<GameTime>
{
    private PhysicsBodyManager _physicsBodyManager;
    private Aether.World _physicsWorld;

    public PhysicsBodyCreationSystem(
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
        var physicsBodyComponent = entity.Get<PhysicsBodyComponent>();

        if (physicsBodyComponent.ReferenceBody == null)
        {
            CreateBody(entity, physicsBodyComponent);
        }
    }

    private void CreateBody(ECS.Entity entity, PhysicsBodyComponent physicsBodyComponent)
    {
        var transformComponent = entity.Get<TransformComponent>();

        Aether.BodyType bodyType;

        if (physicsBodyComponent.IsKinematic)
            bodyType = Aether.BodyType.Kinematic;
        else
            bodyType = Aether.BodyType.Dynamic;


        var body = _physicsWorld.CreateBody(
                transformComponent.Position,
                physicsBodyComponent.Mass,
                bodyType
            );

        if(physicsBodyComponent.LinearDamping.HasValue)
            body.LinearDamping = physicsBodyComponent.LinearDamping.Value;

        if(physicsBodyComponent.AngularDamping.HasValue)
            body.AngularDamping = physicsBodyComponent.AngularDamping.Value;

        Aether.Fixture fixture = body.CreateCircle(physicsBodyComponent.Radius, 1f);
        body.LinearVelocity = physicsBodyComponent.Velocity;

        if(physicsBodyComponent.Restitution.HasValue)
            fixture.Restitution = physicsBodyComponent.Restitution.Value;

        if(physicsBodyComponent.Friction.HasValue)
            fixture.Friction = physicsBodyComponent.Friction.Value;


        int id = _physicsBodyManager.AddBody(body);

        physicsBodyComponent.ReferenceBody = id;

        entity.Set(physicsBodyComponent);
    }
}
