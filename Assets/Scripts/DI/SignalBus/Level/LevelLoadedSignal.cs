using UnityEngine;

namespace DI.SignalBus.Level
{
    public class LevelLoadedSignal
    {
        public int LevelIndex { get; }
        public GameObject LoadedLevel { get; }

        public LevelLoadedSignal(int levelIndex, GameObject loadedLevel)
        {
            LevelIndex = levelIndex;
            LoadedLevel = loadedLevel;
        }
    }
}