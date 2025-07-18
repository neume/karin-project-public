using Microsoft.Xna.Framework.Input;

namespace Karin.Events;

public class KeyPressedEvent : Event
{
    public Keys Key { get; }
    public KeyPressedEvent(Keys key)
    {
        Key = key;
    }

    public override EventType Type => EventType.KeyPressed;

    public override string ToString()
    {
        return $"{Type} Event: {Key}";
    }
}
