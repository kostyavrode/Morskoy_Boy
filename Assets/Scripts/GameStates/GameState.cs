namespace GameStates
{
    public abstract class GameState
    {
        protected GameStateMachine gameStateMachine;

        public GameState(GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }

        public virtual void Enter() {}
        public virtual void Exit() {}
    }
}