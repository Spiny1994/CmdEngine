using CmdEngine.Core.InputManagement;
using CmdEngine.Core.Rendering;
using CmdEngine.Core.Utilities;

namespace CmdEngine.Core.Components.UI;

public class DebugInfo : GraphicBase
{
    private bool _enabled;
    private bool _allowUpdateFps;

    private List<float>? _recordedFps;
    private float _fpsInterval = 1f;
    private float _fpsTimer;
    private int _avarageFps;

    public DebugInfo(GameObject gameObject) : base(gameObject) { }

    public override void Awake()
    {
        _recordedFps = new List<float>();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(Key.VK_F3))
            _enabled = !_enabled;

        _fpsTimer += Time.DeltaTime;

        if (_fpsTimer >= _fpsInterval)
        {
            _allowUpdateFps = true;
            _fpsTimer = 0;
        }
    }

    public override void Draw()
    {
        if (!_enabled)
            return;

        DrawHelper.DrawText(0, 0, $"FPS: {_avarageFps}");
        //DrawHelper.DrawText(7, 0, $"DeltaTime: {Time.DeltaTime}");

        _recordedFps!.Add((1 / Time.DeltaTime));

        if (_recordedFps.Count > 50)
            _recordedFps.RemoveAt(0);

        if (!_allowUpdateFps)
            return;

        _allowUpdateFps = false;

        var total = 0f;
        foreach (var fps in _recordedFps)
            total += fps;

        _avarageFps = (int)(total / _recordedFps.Count);

        _recordedFps.Clear();
    }
}
