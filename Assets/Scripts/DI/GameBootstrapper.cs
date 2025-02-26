using GameStates;
using UnityEngine;
using Zenject;

namespace DI
{
    public class GameBootstrapper : IInitializable
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly Zenject.SignalBus signalBus;

        public GameBootstrapper(GameStateMachine gameStateMachine, Zenject.SignalBus signalBus)
        {
            this.gameStateMachine = gameStateMachine;
            this.signalBus = signalBus;
        }

        public void Initialize()
        {
            Debug.Log("Game Bootstrapper Initialized");

            gameStateMachine.SetState<MenuState>();
        }
    }
}