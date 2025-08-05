namespace Karin.Graphics;

public class AnimationClip
{
    public string Name;
    public List<string> Frames;
    public float Speed;
    public bool Loop;

    public AnimationClip(string name, float speed, bool loop = true)
    {
        Name = name;
        Speed = speed;
        Loop = true;
        Frames = new List<string>();
    }
}
