namespace CmdEngine.Core.Components;

public abstract class GraphicBase : ComponentBase
{
    public GraphicBase(GameObject gameObject) : base(gameObject) { }

    public abstract void Draw();
}
