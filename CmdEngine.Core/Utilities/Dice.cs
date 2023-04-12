namespace CmdEngine.Core.Utilities;

public class Dice
{
    private Random _random;

    public Dice()
    {
        _random = new Random();
    }

    public bool Roll(int percentChance)
    {
        return _random.Next(100) < percentChance;
    }
}
