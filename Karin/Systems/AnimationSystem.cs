using DefaultEcs;
using DefaultEcs.System;
using Karin.Components;
using Karin.Graphics;
using Microsoft.Xna.Framework;

namespace Karin.Systems;

public class AnimationSystem : AEntitySetSystem<GameTime>
{
    public AnimationSystem(World world)
        : base(world.GetEntities().With<SpriteComponent>().With<AnimationComponent>().AsSet())
            
    {
    }

    protected override void Update(GameTime state, in Entity entity)
    {
        var animationComponent = entity.Get<AnimationComponent>();
        var spriteComponent = entity.Get<SpriteComponent>();

        var clip = AnimationManager.GetAnimation(animationComponent.AnimationClipName);

        var timeDiff = animationComponent.NextFrameUntil - state.ElapsedGameTime.TotalMilliseconds;

        if(timeDiff < 0)
        {
            int nextFrameCount = (int)(-timeDiff / clip.Speed) + 1;

            int newFrameIndex = (animationComponent.FrameIndex + nextFrameCount) % clip.Frames.Count;
            animationComponent.FrameIndex = newFrameIndex;
            animationComponent.NextFrameUntil = clip.Speed + (float)timeDiff;

            var frameData = AnimationManager.GetFrame(clip.Frames[newFrameIndex]);
            spriteComponent.SourceRectangle = frameData.SourceRectangle;
            spriteComponent.SpriteName = frameData.TextureName;

            entity.Set(spriteComponent);
            entity.Set(animationComponent);
        }
        else
        {
            animationComponent.NextFrameUntil -= (float)state.ElapsedGameTime.TotalMilliseconds;
            entity.Set(animationComponent);
        }
    }
}


