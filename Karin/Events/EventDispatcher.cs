namespace Karin.Events;

public class EventDispatcher
{
    private readonly Event _event;

    public EventDispatcher(Event eventToDispatch)
    {
        _event = eventToDispatch;
    }

    public void Dispatch<T>(Action<T> action) where T : Event
    {
        if (_event is T eventOfType && !_event.Handled)
        {
            action(eventOfType);
            _event.Handled = true;
        }
    }
}
