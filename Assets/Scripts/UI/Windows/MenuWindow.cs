using System;
using DI.SignalBus;
using DI.SignalBus.States;
using GameStates;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    public class MenuWindow : UIWindow
    {
        private SignalBus signalBus;

        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }
        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayButtonPressed);
            settingsButton.onClick.AddListener(OnSettingsButtonPressed);
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(OnPlayButtonPressed);
            settingsButton.onClick.RemoveListener(OnSettingsButtonPressed);
        }

        public void OnPlayButtonPressed()
        {
            signalBus.Fire(new UIStateChangedSignal(typeof(LevelSelectionWindow)));
            
            signalBus.Fire(new GameStateChangedSignal(typeof(LevelSelectionState)));
        }

        public void OnSettingsButtonPressed()
        {
            signalBus.Fire(new UIStateChangedSignal(typeof(SettingsWindow)));
        }
    }
}