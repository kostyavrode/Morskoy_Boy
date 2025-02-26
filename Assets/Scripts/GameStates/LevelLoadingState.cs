using System.Collections;
using DI.SignalBus.Level;
using Services;
using UnityEngine;
using Zenject;

namespace GameStates
{
    public class LevelLoadingState : GameState
    {
        private readonly SignalBus signalBus;
        
        private CoroutineService coroutineService;
        
        private GameObject loadedLevel;

        private int levelIndex;
        
        public LevelLoadingState(GameStateMachine gameStateMachine, SignalBus signalBus, int levelIndex, CoroutineService coroutineService) : base(gameStateMachine)
        {
            this.signalBus = signalBus;
            this.levelIndex = levelIndex;
            this.coroutineService = coroutineService;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log($"Loading Level {levelIndex}...");
            coroutineService.StartCoroutine(LoadLevelAsync());
        }

        private IEnumerator LoadLevelAsync()
        {
            yield return new WaitForSeconds(1f);

            string levelPath = $"Levels/Level_{levelIndex}";
            GameObject levelPrefab = Resources.Load<GameObject>(levelPath);
            
            if (levelPrefab == null)
            {
                Debug.LogError($"Failed to load level prefab at path: {levelPath}");
                yield break;
            }

            loadedLevel = GameObject.Instantiate(levelPrefab);
            Debug.Log($"Level {levelIndex} loaded successfully");
            
            signalBus.Fire(new LevelLoadedSignal(levelIndex, loadedLevel));
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}