using Zenject;

namespace GameStates
{
    public class PauseState : GameState
    {
        private SignalBus signalBus;
        
        public PauseState(GameStateMachine gameStateMachine, SignalBus signalBus) : base(gameStateMachine)
        {
            this.signalBus = signalBus;
        }
        
        public override void Enter()
        {

        }

        public override void Exit()
        {
            
        }
    }
}