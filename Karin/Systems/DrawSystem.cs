using DefaultEcs;
using DefaultEcs.System;
using Karin.Components;
using Karin.Systems.Services;
using Microsoft.Xna.Framework;

namespace Karin.Systems;

public class DrawSystem : AEntitySetSystem<GameTime>
{
    public List<ISystemService> Services;

    public DrawSystem(World world)
        : base(world.GetEntities().With<DrawInfoComponent>().AsSet())
    {
        Services = new List<ISystemService>();
    }

    protected override void Update(GameTime gameTime, in Entity entity)
    {
        foreach (var service in Services)
        {
            service.Execute(gameTime, entity);
        }

    }

}
