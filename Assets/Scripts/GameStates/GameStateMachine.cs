using System;
using System.Collections.Generic;
using DI.SignalBus;
using DI.SignalBus.Level;
using DI.SignalBus.States;
using Levels;
using Services;
using UnityEngine;
using Zenject;

namespace GameStates
{
    public class GameStateMachine
    {
        private readonly SignalBus signalBus;
        private readonly DiContainer container;
        
        private LevelManager levelManager;
        private CoroutineService coroutineService;
        
        public GameState currentState;
        
        private readonly Dictionary<Type, GameState> stateCache = new();

        public GameStateMachine(SignalBus signalBus, DiContainer container, LevelManager levelManager)
        {
            this.signalBus = signalBus;
            this.container = container;
            this.levelManager = levelManager;
            this.signalBus.Subscribe<LevelSelectedSignal>(OnLevelSelected);
            this.signalBus.Subscribe<LevelLoadedSignal>(OnLevelLoaded);
            signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public void SetState<T>(params object[] args) where T : GameState
        {
            currentState?.Exit();
            
            if (stateCache.TryGetValue(typeof(T), out GameState existingState))
            {
                currentState = existingState;
                Debug.Log($"[GameStateMachine] Switching to cached state: {typeof(T).Name}");
            }
            else
            {
                currentState = container.Instantiate<T>(args);
                stateCache[typeof(T)] = currentState;
                Debug.Log($"[GameStateMachine] Creating and caching new state: {typeof(T).Name}");
            }
            
            currentState.Enter();
        }
        
        public bool TryGetState<T>(out T state) where T : GameState
        {
            if (stateCache.TryGetValue(typeof(T), out GameState existingState))
            {
                state = existingState as T;
                return true;
            }

            state = null;
            return false;
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