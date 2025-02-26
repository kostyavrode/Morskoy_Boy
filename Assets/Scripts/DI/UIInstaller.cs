using DI.SignalBus;
using UI;
using UI.Windows;
using Zenject;

namespace DI
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            // Регистрируем UIManager
            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
        
            // Декларируем сигналы
            Container.DeclareSignal<UIStateChangedSignal>();
            Container.DeclareSignal<UIShowSignal<MenuWindow>>();
            Container.DeclareSignal<UIShowSignal<LevelSelectionWindow>>();
            Container.DeclareSignal<UIShowSignal<InGameWindow>>();
        }
    }
}