namespace DI.SignalBus.Level
{
    public class LevelEndedSignal
    {
        public int LevelIndex { get; }
        public bool IsCompleted { get; }
        public float TimeRemaining { get; }

        public LevelEndedSignal(int levelIndex, bool isCompleted, float timeRemaining = 0)
        {
            LevelIndex = levelIndex;
            IsCompleted = isCompleted;
            TimeRemaining = timeRemaining;
        }
    }
}