using CmdEngine.Core.Data;
using CmdEngine.Core.InputManagement;
using CmdEngine.Core.Components;
using CmdEngine.Core;

namespace CmdEngine.Components.UI;

public class ButtonGroup : GraphicBase
{
    public ButtonGroup(GameObject gameObject) : base(gameObject) { }

    private int _currentIndex = 0;

    public bool WrapSelection { get; } = true;
    public int Spacing { get; } = 3;

    public List<Button> Buttons = new List<Button>();

    public override void Update()
    {
        if (Input.GetKeyDown(Key.VK_DOWN))
            _currentIndex++;
        else if (Input.GetKeyDown(Key.VK_UP))
            _currentIndex--;

        if (Input.GetKeyDown(Key.VK_RETURN))
            Buttons[_currentIndex].ClickAction?.Invoke();

        if (_currentIndex < 0)
            _currentIndex = WrapSelection ? _currentIndex = Buttons.Count - 1 : _currentIndex = 0;
        else if (_currentIndex > Buttons.Count - 1)
            _currentIndex = WrapSelection ? _currentIndex = 0 : _currentIndex = Buttons.Count - 1;
    }

    public override void Draw()
    {
        var index = 0;
        var addedYPos = 0;
        foreach (var button in Buttons)
        {
            button.Draw(GameObject.Transform.Position + new Vector2(0, addedYPos), _currentIndex == index);
            addedYPos += Spacing + 1;
            index++;
        }
    }
}
