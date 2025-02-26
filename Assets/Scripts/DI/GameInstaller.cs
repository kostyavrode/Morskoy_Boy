using DI.SignalBus;
using DI.SignalBus.Level;
using DI.SignalBus.States;
using GameStates;
using Services;
using Zenject;

namespace DI
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            if (!Container.HasBinding<Zenject.SignalBus>()) // ✅ Проверяем, установлен ли уже SignalBus
            {
                SignalBusInstaller.Install(Container); 
            }
            RegisterSignals();
            
            Container.Bind<CoroutineService>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
            
            Container.Bind<GameStateMachine>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameBootstrapper>().AsSingle().NonLazy();
        }
        
        private void RegisterSignals()
        {
            Container.DeclareSignal<GameStateChangedSignal>();
            Container.DeclareSignal<UIStateChangedSignal>();
            Container.DeclareSignal<LevelSelectedSignal>();
            Container.DeclareSignal<LevelLoadedSignal>();
            Container.DeclareSignal<LevelLoadFailedSignal>();
        }
    }
}