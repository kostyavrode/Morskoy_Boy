using DI.SignalBus.Level;
using Game;
using Levels;
using Levels.LevelFactory;
using UnityEngine;
using Zenject;

namespace GameStates
{
    public class PlayingState : GameState
    {
        private readonly SignalBus signalBus;
        
        private LevelFactory levelFactory;
        private ItemSpawner itemSpawner;
        private LevelManager levelManager;
        private GameStateMachine gameStateMachine;

        private int levelIndex;
        
        public PlayingState(GameStateMachine gameStateMachine, SignalBus  signalBus,LevelFactory levelFactory,LevelManager levelManager, int levelIndex=0) : base(gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
            this.signalBus = signalBus;
            this.levelIndex = levelIndex;
            this.levelFactory = levelFactory;
            this.levelManager = levelManager;
        }

        public override void Enter()
        {
            signalBus.Subscribe<LevelEndedSignal>(OnLevelEnded);
            if (levelManager == null)
            {
                levelManager=GameObject.FindObjectOfType<LevelManager>().GetComponent<LevelManager>();
            }
            levelManager.StartLevel();
            if (itemSpawner == null)
            {
                itemSpawner = GameObject.FindObjectOfType<ItemSpawner>().GetComponent<ItemSpawner>();
                itemSpawner.StartSpawning();
            }
            else
            {
                itemSpawner.ResumeSpawning();
            }
        }

        public override void Exit()
        {
            signalBus.Unsubscribe<LevelEndedSignal>(OnLevelEnded);
            itemSpawner.StopSpawning();
            itemSpawner=null;
            if (gameStateMachine.TryGetState<MenuState>(out _))
            {
                Debug.Log("[PlayingState] Returning to menu, destroying level...");
                levelFactory.DestroyLevel();
            }
        }
        
        private void OnLevelEnded(LevelEndedSignal signal)
        {
            Debug.Log($"[PlayingState] Level {signal.LevelIndex} завершен. Успех: {signal.IsCompleted}");

            if (signal.IsCompleted)
            {
                gameStateMachine.SetState<LevelCompleteState>(signal.LevelIndex, signal.TimeRemaining);
            }
            else
            {
                Debug.Log(gameStateMachine);
                gameStateMachine.SetState<LevelFailedState>(signal.LevelIndex);
            }
            levelManager.EndLevel(signal.IsCompleted);
        }
        
        public void SetLevelIndex(int newLevelIndex)
        {
            levelIndex = newLevelIndex;
        }

        public int GetLevelIndex()
        {
            return levelIndex;
        }
    }
}