using System;
using System.Collections;
using DI.SignalBus;
using DI.SignalBus.Level;
using UnityEngine;
using Zenject;

namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private int requiredHolesToPass = 3;
        [SerializeField] private float timeLimit = 60f;

        private int currentHoles = 0;
        private float timeRemaining;
        private bool isLevelActive = false;

        private SignalBus signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        private void Start()
        {
            timeRemaining = timeLimit;
            isLevelActive = true;
            StartCoroutine(LevelTimer());
        }

        private IEnumerator LevelTimer()
        {
            while (isLevelActive)
            {
                timeRemaining -= Time.deltaTime;

                if (timeRemaining <= 0)
                {
                    EndLevel(false);
                    yield break;
                }

                yield return null;
            }
        }

        public void OnBallInHole()
        {
            currentHoles++;
            Debug.Log($"[LevelManager] Забитые лунки: {currentHoles}/{requiredHolesToPass}");

            if (currentHoles >= requiredHolesToPass)
            {
                EndLevel(true);
            }
        }


        private void EndLevel(bool isCompleted)
        {
            isLevelActive = false;
            Debug.Log($"[LevelManager] Уровень {(isCompleted ? "пройден" : "провален")}!");
            
            signalBus.Fire(new LevelEndedSignal(GetCurrentLevelIndex(), isCompleted, timeRemaining));
        }

        private int GetCurrentLevelIndex()
        {
            return 1;
        }
    }
}