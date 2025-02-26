using System;
using DI.SignalBus;
using DI.SignalBus.States;
using GameStates;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    public class PauseWindow : UIWindow
    {
        private SignalBus signalBus;
        
        [SerializeField] private Button continueButton;
        [SerializeField] private Button backToMenuButton;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        private void Awake()
        {
            continueButton.onClick.AddListener(OnContinueButtonPressed);
            backToMenuButton.onClick.AddListener(OnBackToMenuButtonPressed);
        }

        private void OnDestroy()
        {
            continueButton.onClick.RemoveListener(OnContinueButtonPressed);
            backToMenuButton.onClick.RemoveListener(OnContinueButtonPressed);
        }

        private void OnContinueButtonPressed()
        {
            signalBus.Fire(new UIStateChangedSignal(typeof(InGameWindow)));

            signalBus.Fire(new GameStateChangedSignal(typeof(PlayingState)));
        }

        private void OnBackToMenuButtonPressed()
        {
            signalBus.Fire(new UIStateChangedSignal(typeof(MenuWindow)));
            
            signalBus.Fire(new GameStateChangedSignal(typeof(MenuState)));
        }
    }
}