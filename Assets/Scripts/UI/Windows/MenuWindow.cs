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

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }
        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayButtonPressed);
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(OnPlayButtonPressed);
        }

        public void OnPlayButtonPressed()
        {
            signalBus.Fire(new UIStateChangedSignal(typeof(LevelSelectionWindow)));
            
            signalBus.Fire(new GameStateChangedSignal(typeof(LevelSelectionState)));
        }
    }
}