using DI.SignalBus;
using DI.SignalBus.Level;
using DI.SignalBus.States;
using GameStates;
using Levels.LevelFactory;
using Services;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace DI
{
    public class GameInstaller : MonoInstaller
    {

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        public override void InstallBindings()
        {
            if (!Container.HasBinding<Zenject.SignalBus>())
            {
                SignalBusInstaller.Install(Container); 
            }
            RegisterSignals();
            
            Container.Bind<LevelFactory>().AsSingle();
            Container.Bind<SettingsService>().AsSingle().NonLazy();
            
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
            Container.DeclareSignal<LevelEndedSignal>();
            Container.DeclareSignal<SettingsUpdatedSignal>();
        }
    }
}