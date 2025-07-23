using Microsoft.Xna.Framework.Input;

namespace Karin.Events;

public class KeyRepeatedEvent : Event
{
    public Keys Key { get; }
    public KeyRepeatedEvent(Keys key)
    {
        Key = key;
    }

    public override EventType Type => EventType.KeyRepeated;
}
