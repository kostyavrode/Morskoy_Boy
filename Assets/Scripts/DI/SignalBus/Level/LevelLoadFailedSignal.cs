namespace DI.SignalBus.Level
{
    public class LevelLoadFailedSignal
    {
        public int LevelIndex { get; }

        public LevelLoadFailedSignal(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }
}