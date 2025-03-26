namespace DungeonSlime;

public struct GameOptions
{
    public enum SlimeSpeed
    {
        Slow,
        Normal,
        Fast
    }

    public enum GameMode
    {
        Normal,
        Dark
    }

    public SlimeSpeed Speed;
    public GameMode Mode;

    public GameOptions()
    {
        Speed = SlimeSpeed.Normal;
        Mode = GameMode.Normal;
    }
}
