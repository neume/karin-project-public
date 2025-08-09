using Microsoft.Xna.Framework;

namespace Karin.Systems.Services;

public interface ISystemService
{
    void Execute(GameTime gameTime, DefaultEcs.Entity entity);
}
