namespace Karin.Graphics;

public class AnimationControllerManager
{
    private Dictionary<string, AnimationController> _controllers = new();

    public AnimationController? GetController(string name)
    {
        return _controllers.ContainsKey(name) ? _controllers[name] : null;
    }

    public void AddController(string name, AnimationController controller)
    {
        _controllers.Add(name, controller);
    }

    public void AddController(AnimationController controller)
    {
        AddController(controller.Name, controller);
    }

    public void RemoveController(string name)
    {
        _controllers.Remove(name);
    }
}
