using DefaultEcs;
using Microsoft.Xna.Framework;

namespace Karin.AI.GOAP;

public interface IActionExecutor
{
    void Start(Entity entity, int actionId, World world);
    void Update(Entity agent, int actionId, World world, GameTime gameTime, ref bool complete, ref bool valid);
    void Stop(Entity agent, int actionId, World world);
}
