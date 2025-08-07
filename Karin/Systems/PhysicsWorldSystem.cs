using DefaultEcs.System;
using Physics = nkast.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Karin.Physics;

public class PhysicsWorldSystem : ISystem<GameTime>
{
    private Physics.World _physicsWorld;
    private PhysicsBodyManager _physicsBodyManager;

    public PhysicsWorldSystem(Physics.World physicsWorld, PhysicsBodyManager physicsBodyManager)
    {
        _physicsWorld = physicsWorld;
        _physicsBodyManager = physicsBodyManager;
    }

    public bool IsEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Update(GameTime state)
    {
        var dt = state.ElapsedGameTime.TotalSeconds;
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(dt);
        _physicsWorld.Step(timeSpan);
    }
}
