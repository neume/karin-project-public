using DefaultEcs;
using Microsoft.Xna.Framework;

namespace Karin.Graphics;

public class AnimationController
{
    public string Name { get; set; }

    public AnimationController()
    {
        if(Name == null)
            Name = this.GetType().Name;
    }

    public virtual void Execute(GameTime gameTime, Entity entity) {}
}
