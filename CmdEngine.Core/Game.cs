using CmdEngine.Core.Components;
using CmdEngine.Core.Rendering;
using CmdEngine.Core.Scenes;
using CmdEngine.Core.Utilities;
using CmdEngine.Core.InputManagement;

namespace CmdEngine.Core;

public class Game
{
    public static bool Running;

    public void GameLoop()
    {
        Initialize();

        Running = true;

        while (Running)
        {
            var startTime = DateTime.Now;

            // Filter.
            var comps = new List<ComponentBase>();
            var graphics = new List<GraphicBase>();
            foreach (var gameObject in SceneManager.CurrentScene.GameObjects)
            {
                comps.AddRange(gameObject.GetComponents());
                graphics.AddRange(gameObject.GetComponents().Where(x => x is GraphicBase).Cast<GraphicBase>().ToArray());
            }

            // Update.
            foreach (var comp in comps)
            {
                if (comp.GameObject.IsActive)
                    comp.Update();
            }

            // Clear buffer.
            ConsoleBuffer.Clear();

            // Draw to buffer.
            foreach (var graphic in graphics)
            {
                if (graphic.GameObject.IsActive)
                    graphic.Draw();
            }

            // Render buffer to screen.
            ConsoleBuffer.Render();

            // Create and destroy GameObjects.
            SceneManager.ResolveGameObjects();

            // This is needed to make input behave correctly.
            Input.MarkEndOfFrame();

            var endTime = DateTime.Now;
            Time.DeltaTime = (float)(endTime - startTime).TotalSeconds;
        }
    }

    private void Initialize()
    {
        // This disables minimize, maximize, and resize.
        WindowHelper.DisableAllMenus();
    }
}
