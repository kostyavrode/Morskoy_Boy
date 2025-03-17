using DI.SignalBus;
using DI.SignalBus.Level;
using DI.SignalBus.States;
using Game;
using GameStates;
using Levels;
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
        [SerializeField] private ItemSpawner itemSpawner;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private ItemSelector itemSelector;
        public override void InstallBindings()
        {
            if (!Container.HasBinding<Zenject.SignalBus>())
            {
                SignalBusInstaller.Install(Container); 
            }
            RegisterSignals();
            Container.Bind<LevelManager>().FromInstance(levelManager).AsSingle();
            Container.Bind<ItemSelector>().FromInstance(itemSelector).AsSingle();
            Container.Bind<LevelFactory>().AsSingle();
            Container.Bind<SettingsService>().AsSingle().NonLazy();
            
            Container.Bind<CoroutineService>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
            
            Container.Bind<GameStateMachine>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<GameBootstrapper>().AsSingle().NonLazy();
            
            Container.Bind<ItemSpawner>().FromInstance(itemSpawner).AsSingle();
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