using System;
using System.Collections.Generic;
using DI.SignalBus;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private List<UIWindow> windows; // Список всех окон

        private readonly Dictionary<Type, UIWindow> windowDict = new();
        private SignalBus signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;

            foreach (var window in windows)
            {
                windowDict[window.GetType()] = window;
                window.Hide(); // Все окна скрыты при старте
            }

            // Подписка на сигналы
            signalBus.Subscribe<UIStateChangedSignal>(OnUIStateChanged);
        }

        private void OnUIStateChanged(UIStateChangedSignal signal)
        {
            foreach (var window in windowDict.Values)
            {
                window.Hide();
            }

            if (windowDict.TryGetValue(signal.WindowType, out var selectedWindow))
            {
                selectedWindow.Show();
            }
            else
            {
                Debug.LogError($"❌ UIManager: окно {signal.WindowType.Name} не найдено!");
            }
        }

    }
}