using System;
using DG.Tweening;
using UnityEngine;
using Zenject;
using DI.SignalBus.States; // Пространство имен для сигналов

namespace Game
{
    public class ItemMover : MonoBehaviour
    {
        public float speed = 2f;
        public bool isStopped = false;
        private bool isSelected = false;
        public int line;
        private Vector3 originalScale;

        private ItemSelector _itemSelector;
        public SignalBus _signalBus; // Добавляем SignalBus для подписки

        [Inject]
        public void Construct(ItemSelector itemSelector, SignalBus signalBus)
        {
            _itemSelector = itemSelector;
            _signalBus = signalBus;

            // Подписка на сигнал смены состояния
            
        }

        private void Start()
        {
            originalScale = transform.localScale;
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }
        
        private void OnGameStateChanged(GameStateChangedSignal signal)
        {
            if (signal.StateType == typeof(GameStates.PauseState))
            {
                Stop(); // Останавливаем движение при паузе
            }
            else if (signal.StateType == typeof(GameStates.PlayingState))
            {
                Resume(); // Продолжаем движение при возвращении в игру
            }
        }

        public void Stop()
        {
            isStopped = true;
        }

        public void Resume()
        {
            isStopped = false;
            //ScaleDown();
        }

        void Update()
        {
            if (!isStopped)
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
        }
        public void ScaleUp()
        {
            transform.DOScale(originalScale * 1.2f, 0.3f) // Увеличиваем размер на 20%
                .SetEase(Ease.OutBounce); // Делаем плавное увеличение
        }

        public void ScaleDown()
        {
            try
            {
                transform.DOScale(originalScale, 0.3f) // Возвращаемся к исходному размеру
                    .SetEase(Ease.InOutQuad);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        void OnMouseDown()
        {
            {
                bool isLine1 = transform.position.x != -0.617f; // Если слева – первая линия
                ItemSelector.Instance.SelectItem(this, isLine1);
                isSelected = true;
                ScaleUp();
            }
        }
    }
}
