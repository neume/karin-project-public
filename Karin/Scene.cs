using ECS = DefaultEcs;
using Microsoft.Xna.Framework;
using Aether = nkast.Aether.Physics2D.Dynamics;
using Karin.Physics;

namespace Karin;

public class Scene
{
    public ECS.World World { get; private set; } = new ECS.World();
    public Aether.World PhysicsWorld { get; private set; } = new Aether.World();
    public PhysicsBodyManager PhysicsBodyManager { get; private set; }

    public Scene()
    {
        PhysicsBodyManager = new PhysicsBodyManager(PhysicsWorld);
    }

    public virtual void Start()
    {
    }

    public virtual void LoadContent()
    {
    }

    public virtual void BeforeUpdate(GameTime gameTime)
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void AfterUpdate(GameTime gameTime)
    {
    }

    public virtual void FixedUpdate(GameTime gameTime)
    {
    }

    public virtual void Draw(GameTime gameTime)
    {
    }

    public virtual void Destroy()
    {
    }
}
