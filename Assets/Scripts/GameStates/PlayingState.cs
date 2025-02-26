using Zenject;

namespace GameStates
{
    public class PlayingState : GameState
    {
        private readonly SignalBus signalBus;

        private int levelIndex;
        public PlayingState(GameStateMachine gameStateMachine, SignalBus  signalBus, int levelIndex=0) : base(gameStateMachine)
        {
            this.signalBus = signalBus;
            this.levelIndex = levelIndex;
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}