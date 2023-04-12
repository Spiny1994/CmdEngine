using CmdEngine.Core.Components;
using System.Diagnostics;

namespace CmdEngine.Core.Scenes;

public class SceneManager
{
    private readonly static List<Scene> _scenes;

    private readonly static List<GameObject> _gameObjectsToAdd;
    private readonly static List<GameObject> _gameObjectsToRemove;
    private readonly static List<GameObject> _initializedGameObjects;

    private static Scene? _currentScene;

    public static Scene CurrentScene
    {
        get => _currentScene!;
        set
        {
            if (_currentScene == value)
                return;

            _currentScene = value;

            Debug.Print($"Current scene changed to: {_currentScene.Name}.");
        }
    }

    static SceneManager()
    {
        _scenes = new List<Scene>();
        _gameObjectsToAdd = new List<GameObject>();
        _gameObjectsToRemove = new List<GameObject>();
        _initializedGameObjects = new List<GameObject>();

        CurrentScene = new Scene("EmptyScene");
    }

    /// <summary>
    /// Adds and removes GameObjects from the current scene and raises applicable functions in their components.
    /// </summary>
    public static void ResolveGameObjects()
    {
        var objectsToAdd = new List<GameObject>(_gameObjectsToAdd);
        var objectsToRemove = new List<GameObject>(_gameObjectsToRemove);

        // Clear lists.
        _gameObjectsToAdd.Clear();
        _gameObjectsToRemove.Clear();

        foreach (var gameObject in objectsToRemove)
        {
            if (CurrentScene.GameObjects.Contains(gameObject))
            {
                // Raise OnDestroy.
                foreach (var comp in gameObject.GetComponents())
                    comp.OnDestroy();

                // Remove from scene.
                CurrentScene.GameObjects.Remove(gameObject);
                Debug.Print($"Destroyed GameObject: {gameObject.Name}.");

                // Remove from initialized objects.
                if (_initializedGameObjects.Contains(gameObject))
                    _initializedGameObjects.Remove(gameObject);
            }
        }

        // Add GameObjects to current scene.
        foreach (var gameObject in objectsToAdd)
        {
            CurrentScene.GameObjects.Add(gameObject);
            Debug.Print($"Added GameObject: {gameObject.Name}.");
        }

        // Raise Awake.
        foreach (var gameObject in objectsToAdd)
        {
            if (gameObject.IsActive && !_initializedGameObjects.Contains(gameObject))
            {
                foreach (var comp in gameObject.GetComponents())
                    comp.Awake();
            }
        }

        // Raise Start.
        foreach (var gameObject in objectsToAdd)
        {
            if (gameObject.IsActive && !_initializedGameObjects.Contains(gameObject))
            {
                foreach (var comp in gameObject.GetComponents())
                    comp.Start();
            }
        }

        // Add to initialized objects.
        foreach (var gameObject in objectsToAdd)
        {
            if (!_initializedGameObjects.Contains(gameObject))
                _initializedGameObjects.Add(gameObject);
        }

        // Raise OnEnable/OnDisable.
        foreach (var gameObject in objectsToAdd)
        {
            foreach (var comp in gameObject.GetComponents())
            {
                if (gameObject.IsActive)
                    comp.OnEnable();
                else
                    comp.OnDisable();
            }
        }
    }

    /// <summary>
    /// Registers a scene so it can be loaded by name.
    /// </summary>
    public static void RegisterScene(Scene scene)
    {
        if (_scenes.Any(x => x.Name == scene.Name))
        {
            Debug.Print("Scenes have to have a unique name. Can't register scene.");
            return;
        }

        _scenes.Add(scene);
    }

    /// <summary>
    /// Loads a scene by name.
    /// </summary>
    public static void LoadScene(string name)
    {
        var scene = _scenes.FirstOrDefault(x => x.Name == name);

        if (scene == null)
        {
            Debug.Print($"Couldn't find scene with name: {name}.");
            return;
        }

        LoadScene(scene);
    }

    /// <summary>
    /// Loads a scene by reference.
    /// </summary>
    public static void LoadScene(Scene scene)
    {
        if (CurrentScene != null)
            CurrentScene.Unload();

        scene.Load();
        CurrentScene = scene;
    }

    /// <summary>
    /// Adds a GameObject to the scene when possible.
    /// </summary>
    public static GameObject Instantiate(GameObject gameObject)
    {
        var instantiatedGameObject = new GameObject(gameObject.Name + "(Clone)");

        foreach (var component in gameObject.GetComponents())
        {
            var clone = component.CloneTo(instantiatedGameObject);

            instantiatedGameObject.AddComponent(clone);
        }

        instantiatedGameObject.Transform.SetPosition(gameObject.Transform.Position);

        _gameObjectsToAdd.Add(instantiatedGameObject);
        return instantiatedGameObject;
    }

    /// <summary>
    /// Removes a GameObject from the scene when possible.
    /// </summary>
    public static void Destroy(GameObject gameObject)
    {
        _gameObjectsToRemove.Add(gameObject);
    }

    /// <summary>
    /// Called by GameObjects when their active state changes.
    /// Makes sure their Awake/Start/OnEnable/OnDisable functions are called if applicable.
    /// </summary>
    public static void RegisterGameObjectActiveStateChanged(GameObject gameObject, bool active)
    {
        // Get all the components.
        var comps = new List<ComponentBase>();
        comps.AddRange(gameObject.GetComponents());

        foreach (var comp in comps)
        {
            if (active)
                comp.OnEnable();
            else
                comp.OnDisable();
        }

        // Is the GameObject already initialized?
        if (_initializedGameObjects.Contains(gameObject))
            return;

        // Was it not set as active?
        if (!active)
            return;

        foreach (var comp in comps)
            comp.Awake();

        foreach (var comp in comps)
            comp.Start();

        _initializedGameObjects.Add(gameObject);
    }
}
