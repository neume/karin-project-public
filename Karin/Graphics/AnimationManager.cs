using Microsoft.Xna.Framework;

namespace Karin.Graphics;

public class AnimationManager
{
    public static Dictionary<string, AnimationClip> Animations = new Dictionary<string, AnimationClip>();
    public static Dictionary<string, FrameData> Frames = new Dictionary<string, FrameData>();

    public static void AddAnimation(string name, AnimationClip animationClip)
    {
        Animations.Add(name, animationClip);
    }

    public static void AddFrame(string name, string textureName, Rectangle sourceRectangle)
    {
        Frames.Add(name, new FrameData() { TextureName = textureName, SourceRectangle = sourceRectangle });
    }

    public static FrameData GetFrame(string name)
    {
        return Frames[name];
    }

    public static AnimationClip GetAnimation(string name)
    {
        return Animations[name];
    }
}
