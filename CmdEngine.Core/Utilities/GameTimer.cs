namespace CmdEngine.Core.Utilities;

public class GameTimer
{
    private readonly Action _callback;

    private float _timer;

    public float Interval { get; set; }

    public GameTimer(float interval, Action callback)
    {
        Interval = interval;
        _callback = callback;
    }

    public bool Tick()
    {
        if (Interval <= 0)
            Interval = 0.001f;

        _timer += Time.DeltaTime;

        if (_timer >= Interval)
        {
            _callback?.Invoke();
            _timer = 0;
            return true;
        }

        return false;
    }

    public void Reset()
    {
        _timer = 0;
    }
}
