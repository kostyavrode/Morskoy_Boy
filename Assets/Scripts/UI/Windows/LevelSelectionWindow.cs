using System;
using DI.SignalBus;
using DI.SignalBus.States;
using GameStates;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    public class LevelSelectionWindow : UIWindow
    {
        private SignalBus signalBus;

        [SerializeField] private Button backButton;
        [SerializeField] private Button Level1Button;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        private void Awake()
        {
            backButton.onClick.AddListener(OnBackButtonPressed);
            Level1Button.onClick.AddListener(OnLevel1ButtonPressed);
        }

        private void OnDestroy()
        {
            backButton.onClick.RemoveListener(OnBackButtonPressed);
            Level1Button.onClick.RemoveListener(OnLevel1ButtonPressed);
        }

        public void OnBackButtonPressed()
        {
            signalBus.Fire(new UIStateChangedSignal(typeof(MenuWindow)));
            
            signalBus.Fire(new GameStateChangedSignal(typeof(MenuState)));
        }

        public void OnLevel1ButtonPressed()
        {
            signalBus.Fire(new UIStateChangedSignal(typeof(InGameWindow)));
            
            signalBus.Fire(new GameStateChangedSignal(typeof(LevelLoadingState),1));
        }
    }
}