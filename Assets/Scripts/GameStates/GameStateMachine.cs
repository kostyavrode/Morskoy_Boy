using DI.SignalBus;
using DI.SignalBus.Level;
using DI.SignalBus.States;
using Services;
using UnityEngine;
using Zenject;

namespace GameStates
{
    public class GameStateMachine
    {
        private readonly SignalBus signalBus;
        private readonly DiContainer container;
        
        private CoroutineService coroutineService;
        
        private GameState currentState;

        public GameStateMachine(SignalBus signalBus, DiContainer container)
        {
            this.signalBus = signalBus;
            this.container = container;
            this.signalBus.Subscribe<LevelSelectedSignal>(OnLevelSelected);
            this.signalBus.Subscribe<LevelLoadedSignal>(OnLevelLoaded);
            signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public void SetState<T>(params object[] args) where T : GameState
        {
            currentState?.Exit();
            currentState = container.Instantiate<T>(args);
            currentState.Enter();
            Debug.Log($"GameStateMachine switched to {typeof(T).Name}");
        }
        
        private void OnGameStateChanged(GameStateChangedSignal signal)
        {
            var setStateMethod = typeof(GameStateMachine)
                .GetMethod(nameof(SetState))
                ?.MakeGenericMethod(signal.StateType);

            setStateMethod?.Invoke(this, new object[] { signal.Args });
        }
        
        private void OnLevelSelected(LevelSelectedSignal signal)
        {
            SetState<LevelLoadingState>(this, signalBus, signal.LevelIndex, coroutineService);
        }

        private void OnLevelLoaded(LevelLoadedSignal signal)
        {
            SetState<PlayingState>(this, signalBus, signal.LevelIndex);
        }
    }
}