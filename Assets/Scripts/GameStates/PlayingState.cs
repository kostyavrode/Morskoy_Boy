using DI.SignalBus.Level;
using Levels.LevelFactory;
using UnityEngine;
using Zenject;

namespace GameStates
{
    public class PlayingState : GameState
    {
        private readonly SignalBus signalBus;
        
        private LevelFactory levelFactory;

        private int levelIndex;
        
        public PlayingState(GameStateMachine gameStateMachine, SignalBus  signalBus,LevelFactory levelFactory, int levelIndex=0) : base(gameStateMachine)
        {
            this.signalBus = signalBus;
            this.levelIndex = levelIndex;
            this.levelFactory = levelFactory;
        }

        public override void Enter()
        {
            signalBus.Subscribe<LevelEndedSignal>(OnLevelEnded);
        }

        public override void Exit()
        {
            signalBus.Unsubscribe<LevelEndedSignal>(OnLevelEnded);
            
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
                gameStateMachine.SetState<LevelFailedState>(signal.LevelIndex);
            }
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