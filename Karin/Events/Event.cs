using Microsoft.Xna.Framework.Input;

namespace Karin.Events;

public abstract class Event 
{
    public bool Handled { get; set; } = false;
    public abstract EventType Type { get; }

    virtual public string ToString()
    {
        return $"{Type} Event";
    }
}

public enum EventType
{
    None = 0,
    KeyDown,
    KeyPressed,
    KeyReleased,
    MouseButtonPressed,
    MouseButtonReleased,
    MouseMoved,
    MouseScrolled,
    WindowResized,
    WindowClosed,
}
