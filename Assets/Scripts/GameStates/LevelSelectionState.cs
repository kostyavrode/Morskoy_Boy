using DI.SignalBus;
using UnityEngine;
using Zenject;

namespace GameStates
{
    public class LevelSelectionState : GameState
    {
        
        private readonly SignalBus signalBus;

        public LevelSelectionState(GameStateMachine gameStateMachine, SignalBus signalBus) : base(gameStateMachine)
        {
            this.signalBus = signalBus;
        }

        public override void Enter()
        {
            base.Enter();
            signalBus.Subscribe<LevelSelectedSignal>(OnLevelSelected);
        }

        private void OnLevelSelected(LevelSelectedSignal signal)
        {
            Debug.Log($"Level Selected: {signal.LevelIndex}");
        }

        public override void Exit()
        {
            base.Exit();
            signalBus.Unsubscribe<LevelSelectedSignal>(OnLevelSelected);
        }
    }
}