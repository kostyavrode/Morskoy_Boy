namespace DI.SignalBus.States
{
    public class GameStateChangedSignal
    {
        public System.Type StateType { get; }
        public object[] Args { get; }

        public GameStateChangedSignal(System.Type stateType, params object[] args)
        {
            StateType = stateType;
            Args = args;
        }
    }

}