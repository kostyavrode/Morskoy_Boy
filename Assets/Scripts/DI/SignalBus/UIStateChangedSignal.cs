using UI.Windows;

namespace DI.SignalBus
{
    public class UIStateChangedSignal
    {
        public System.Type WindowType;

        public UIStateChangedSignal(System.Type windowType)
        {
            WindowType = windowType;
        }
    }

    public class UIShowSignal<T> where T : UIWindow { }
    public class UIHideSignal<T> where T : UIWindow { }
}