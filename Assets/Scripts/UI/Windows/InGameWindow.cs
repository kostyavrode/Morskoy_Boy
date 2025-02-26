using System;
using DI.SignalBus;
using DI.SignalBus.States;
using GameStates;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using PauseState = GameStates.PauseState;

namespace UI.Windows
{
    public class InGameWindow : UIWindow
    {
        private SignalBus signalBus;
        
        [SerializeField] private Button pauseButton;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        private void Awake()
        {
            pauseButton.onClick.AddListener(OnPauseButtonPressed);
        }

        private void OnDestroy()
        {
            pauseButton.onClick.RemoveListener(OnPauseButtonPressed);
        }

        public void OnPauseButtonPressed()
        {
            signalBus.Fire(new UIStateChangedSignal(typeof(PauseWindow)));
            
            signalBus.Fire(new GameStateChangedSignal(typeof(PauseState)));
        }
    }
}