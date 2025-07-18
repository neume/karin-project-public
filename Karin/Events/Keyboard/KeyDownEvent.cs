using Microsoft.Xna.Framework.Input;

namespace Karin.Events;

public class KeyDownEvent : Event
{
    public Keys Key { get; }
    public KeyDownEvent(Keys key)
    {
        Key = key;
    }

    public override EventType Type => EventType.KeyDown;
}
