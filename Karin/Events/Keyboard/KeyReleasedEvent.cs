using Microsoft.Xna.Framework.Input;

namespace Karin.Events;

public class KeyReleasedEvent : Event
{
    public Keys Key { get; }
    public KeyReleasedEvent(Keys key)
    {
        Key = key;
    }

    public override EventType Type => EventType.KeyReleased;
}
