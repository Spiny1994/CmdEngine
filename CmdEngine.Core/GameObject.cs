using CmdEngine.Core.Components;
using CmdEngine.Core.Scenes;
using System.Diagnostics;

namespace CmdEngine.Core;

public class GameObject
{
    public Transform Transform { get; private set; }

    public string Name { get; set; }

    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (_isActive == value)
                return;

            _isActive = value;

            SceneManager.RegisterGameObjectActiveStateChanged(this, _isActive);
        }
    }

    private bool _isActive = true;

    private List<ComponentBase> _components { get; }


    public GameObject(string name = "GameObject")
    {
        _components = new List<ComponentBase>();
        Transform = AddComponent<Transform>();

        Name = name;
    }

    public T AddComponent<T>() where T : ComponentBase
    {
        var type = typeof(T);
        var component = Activator.CreateInstance(type, new GameObject[] { this }) as ComponentBase;

        if (component == null)
        {
            Debug.Print($"Failed to create component of type '{nameof(T)}'.");
            return null!;
        }

        return (T)AddComponent(component)!;
    }

    public ComponentBase AddComponent(ComponentBase component)
    {
        if (component.GameObject != this)
        {
            Debug.Print("You can only add components with this as assigned GameObject!");
            return null!;
        }

        _components.Add(component);

        return component;
    }

    public void RemoveComponent(ComponentBase component)
    {
        if (component.GameObject != this)
        {
            Debug.Print("You can only remove components with this as assigned GameObject!");
            return;
        }

        _components.Remove(component);
    }


    public T? GetComponent<T>() where T : ComponentBase
    {
        var component = _components.FirstOrDefault(x => x.GetType() == typeof(T));

        if (component == null)
        {
            Debug.Print($"No component found of type '{nameof(T)}'.");
            return null;
        }

        return component as T;
    }

    public List<ComponentBase> GetComponents()
    {
        return _components;
    }

    public static GameObject? Find(string name)
    {
        return SceneManager
            .CurrentScene
            .GameObjects
            .FirstOrDefault(x => x.Name == name || x.Name == name + "(Clone)");
    }
}
