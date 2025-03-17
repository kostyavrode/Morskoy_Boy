using GameStates;
using Levels;
using Services;
using UnityEngine;
using Zenject;

namespace DI
{
    public class GameBootstrapper : IInitializable
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly SettingsService settingsService;
        private readonly Zenject.SignalBus signalBus;

        public GameBootstrapper(GameStateMachine gameStateMachine, Zenject.SignalBus signalBus, SettingsService settingsService)
        {
            this.gameStateMachine = gameStateMachine;
            this.signalBus = signalBus;
            this.settingsService = settingsService;
        }

        public void Initialize()
        {
            Debug.Log("Game Bootstrapper Initialized");

            AudioSource musicSource = GameObject.FindWithTag("MusicSource")?.GetComponent<AudioSource>();
            AudioSource sfxSource = GameObject.FindWithTag("SFXSource")?.GetComponent<AudioSource>();
            
            settingsService.Initialize(musicSource, sfxSource);
            
            gameStateMachine.SetState<MenuState>();
        }
    }
}