namespace CmdEngine.Core.Scenes;

public class Scene
{
    public string Name { get; }

    public List<GameObject> SceneObjects { get; }
    public List<GameObject> GameObjects { get; }

    public Scene(string name)
    {
        SceneObjects = new List<GameObject>();
        GameObjects = new List<GameObject>();

        Name = name;

        SceneManager.RegisterScene(this);
    }

    public Scene Load()
    {
        GameObjects.Clear();

        foreach (var sceneObject in SceneObjects)
            SceneManager.Instantiate(sceneObject);

        return this;
    }

    public void Unload()
    {
        GameObjects.Clear();
    }
}
