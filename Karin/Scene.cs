using ECS = DefaultEcs;
using Microsoft.Xna.Framework;
using Aether = nkast.Aether.Physics2D.Dynamics;
using Karin.Physics;
using Karin.Core;
using Karin.Spatials;
using DefaultEcs;

namespace Karin;

public class Scene
{
    public ECS.World World { get; private set; } = new ECS.World();
    public Aether.World PhysicsWorld { get; private set; } = new Aether.World();
    public PhysicsBodyManager PhysicsBodyManager { get; private set; }
    public SystemsManager DrawSystemsManager;
    public SystemsManager UpdateSystemsManager;
    public SystemsManager FixedUpdateSystemsManager;
    public IdAllocator EntityIdAllocator { get; private set; } = new IdAllocator();
    public GridPartitioning? Grid { get; private set; }

    public Scene()
    {
        DrawSystemsManager = new SystemsManager();
        UpdateSystemsManager = new SystemsManager();
        FixedUpdateSystemsManager = new SystemsManager();
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

    protected void InitGrid()
    {
        Grid = new GridPartitioning(10);
    }

    public List<Entity> FindEntities(float x, float y, float range)
    {
        if (Grid == null)
            return new List<Entity>();

        return Grid.Query(x, y, range).Select(p => p.Entity).ToList();
    }

    public List<Entity> FindEntities(float x, float y, float range, EntitySet set)
    {
        List<Entity> nearbyEntities = FindEntities(x, y, range);
        List<Entity> result = new List<Entity>(nearbyEntities.Count);

        if(nearbyEntities.Count > set.Count)
        {
            foreach(var entity in nearbyEntities)
                if(set.Contains(entity))
                    result.Add(entity);

        }
        else
        {
            var nearbySet = new HashSet<Entity>(nearbyEntities);

            foreach(ref readonly var entity in set.GetEntities())
                if(nearbySet.Contains(entity))
                    result.Add(entity);
        }

        return result;
    }
}
