using CmdEngine.Core.Data;
using CmdEngine.Core.Scenes;

namespace CmdEngine.Core.Components;

public abstract class ComponentBase
{
    public GameObject GameObject { get; private set; }

    public ComponentBase(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public GameObject Instantiate(GameObject gameObject, Vector2 position = new Vector2())
    {
        gameObject.Transform.SetPosition(position);
        return SceneManager.Instantiate(gameObject);
    }

    public void Destroy(GameObject? gameObject = null)
    {
        if (gameObject == null)
            gameObject = GameObject;

        SceneManager.Destroy(gameObject);
    }

    public ComponentBase CloneTo(GameObject gameObject)
    {
        var clone = (ComponentBase)MemberwiseClone();
        clone.GameObject = gameObject;

        return clone;
    }

    public virtual void Awake() { }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void OnDestroy() { }
}
