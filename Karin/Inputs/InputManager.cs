using Karin.Events;
using Microsoft.Xna.Framework.Input;

namespace Karin.Inputs;

public class InputManager
{
    private KeyboardState _currentKeyboardState;
    private KeyboardState _previousKeyboardState;

    public delegate void EventCallback(Event e);
    public event EventCallback? OnEvent;

    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();

        CheckForKeyEvents();
    }

    private void CheckForKeyEvents()
    {
        foreach (Keys key in Enum.GetValues(typeof(Keys)))
        {
            if (_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key))
            {
                OnKeyPressed(key);
            }
            else if (_currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key))
            {
                OnKeyReleased(key);
            }
        }
    }

    private void OnKeyPressed(Keys key)
    {
        var keyPressedEvent = new KeyPressedEvent(key);
        OnEvent?.Invoke(keyPressedEvent);
    }

    private void OnKeyReleased(Keys key)
    {
        var keyReleasedEvent = new KeyReleasedEvent(key);
        OnEvent?.Invoke(keyReleasedEvent);
    }
}
