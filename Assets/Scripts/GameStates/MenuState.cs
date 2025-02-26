using DI.SignalBus;
using UI.Windows;
using Zenject;

namespace GameStates
{
    public class MenuState : GameState
    {
        private SignalBus signalBus;
        public MenuState(GameStateMachine gameStateMachine, SignalBus signalBus) : base(gameStateMachine)
        {
            this.signalBus = signalBus;
        }

        public override void Enter()
        {
            signalBus.Fire(new UIStateChangedSignal(typeof(MenuWindow)));
        }

        public override void Exit()
        {
            
        }
    }
}