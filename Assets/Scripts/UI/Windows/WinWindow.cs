using DI.SignalBus;
using DI.SignalBus.States;
using GameStates;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    public class WinWindow : UIWindow
    {
        private SignalBus signalBus;
        
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button backToMenuButton;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        private void Awake()
        {
            nextLevelButton.onClick.AddListener(OnNextLevelButtonPressed);
            backToMenuButton.onClick.AddListener(OnBackToMenuButtonPressed);
        }

        private void OnDestroy()
        {
            nextLevelButton.onClick.RemoveListener(OnNextLevelButtonPressed);
            backToMenuButton.onClick.RemoveListener(OnNextLevelButtonPressed);
        }

        private void OnNextLevelButtonPressed()
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