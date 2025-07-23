using Karin.Events;
using Microsoft.Xna.Framework.Input;

namespace Karin.Inputs;

public class InputManager
{
    private KeyboardState _currentKeyboardState;
    private KeyboardState _previousKeyboardState;

    // API for input polling
    public List<Event> Events { get; private set; } = new();

    public delegate void EventCallback(Event e);
    public event EventCallback? OnEvent;

    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();

        Events.Clear();
        CheckForKeyEvents();
    }

    private void CheckForKeyEvents()
    {
        foreach (Keys key in Enum.GetValues(typeof(Keys)))
        {
            if (_currentKeyboardState.IsKeyDown(key))
            {
                OnKeyDown(key);
                Events.Add(new KeyDownEvent(key));
            }

            if (_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key))
            {
                OnKeyPressed(key);
                Events.Add(new KeyPressedEvent(key));
            }
            else if (_currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key))
            {
                OnKeyReleased(key);
                Events.Add(new KeyReleasedEvent(key));
            }
            else if (_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyDown(key))
            {
                OnKeyRepeated(key);
                Events.Add(new KeyRepeatedEvent(key));
            }
        }
    }

    private void OnKeyPressed(Keys key)
        => OnEvent?.Invoke(new KeyPressedEvent(key));

    private void OnKeyReleased(Keys key)
        => OnEvent?.Invoke(new KeyReleasedEvent(key));

    private void OnKeyRepeated(Keys key)
        => OnEvent?.Invoke(new KeyRepeatedEvent(key));

    private void OnKeyDown(Keys key)
        => OnEvent?.Invoke(new KeyDownEvent(key));
}
