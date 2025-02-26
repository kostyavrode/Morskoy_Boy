using DI.SignalBus;
using DI.SignalBus.States;
using GameStates;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    public class LoseWindow : UIWindow
    {
        private SignalBus signalBus;
        
        [SerializeField] private Button restartButton;
        [SerializeField] private Button backToMenuButton;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestartButtonPressed);
            backToMenuButton.onClick.AddListener(OnBackToMenuButtonPressed);
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(OnRestartButtonPressed);
            backToMenuButton.onClick.RemoveListener(OnRestartButtonPressed);
        }

        private void OnRestartButtonPressed()
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