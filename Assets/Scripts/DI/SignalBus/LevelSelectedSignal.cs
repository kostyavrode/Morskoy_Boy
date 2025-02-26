namespace DI.SignalBus
{
    public class LevelSelectedSignal
    {
        public int LevelIndex { get; }

        public LevelSelectedSignal(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }
}