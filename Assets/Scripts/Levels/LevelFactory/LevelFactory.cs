using DI.SignalBus;
using UnityEngine;
using Zenject;

namespace Levels.LevelFactory
{
    public class LevelFactory
    {
        private readonly SignalBus signalBus;
        private readonly DiContainer container;

        private GameObject currentLevel;

        public LevelFactory(SignalBus signalBus, DiContainer container)
        {
            this.signalBus = signalBus;
            this.container = container;
        }
        
        public GameObject CreateLevel(int levelIndex)
        {
            string levelPath = $"Levels/Level_{levelIndex}";
            GameObject levelPrefab = Resources.Load<GameObject>(levelPath);

            if (levelPrefab == null)
            {
                Debug.LogError($"[LevelFactory] Failed to load level prefab at path: {levelPath}");
                return null;
            }
            
            GameObject levelInstance = GameObject.Instantiate(levelPrefab);
            
            currentLevel = levelInstance;
            
            LevelManager levelManager = container.InstantiateComponent<LevelManager>(levelInstance);

            Debug.Log($"[LevelFactory] Level {levelIndex} created successfully");
            return levelInstance;
        }
        
        public void DestroyLevel()
        {
            if (currentLevel != null)
            {
                GameObject.Destroy(currentLevel);
                Debug.Log("[LevelFactory] Level destroyed successfully");
            }
        }
    }
}