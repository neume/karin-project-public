using DefaultEcs;
using Microsoft.Xna.Framework;

namespace Karin;

public class Scene
{
    public World World { get; private set; } = new World();

    public Scene()
    {
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
