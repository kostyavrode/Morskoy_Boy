using System;
using System.Collections;
using DI.SignalBus;
using DI.SignalBus.Level;
using DI.SignalBus.States;
using GameStates;
using TMPro;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        public float levelTime = 30f;
        private float timeRemaining;
        private bool isLevelActive = false;
        public TMP_Text timerText;
        public SignalBus signalBus;
        public Image hpBar;
        public int levelIndex;


        [Inject]
        public void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        void Start()
        {
            //StartLevel();
            timeRemaining = levelTime;
        }

        public void StartLevel()
        {
            timeRemaining = levelTime;
            isLevelActive = true;
            StartCoroutine(LevelTimer());
        }

        private IEnumerator LevelTimer()
        {
            while (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                //timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
                hpBar.fillAmount = timeRemaining / levelTime;
                yield return null;
            }

            EndLevel(false);
        }

        public void EndLevel(bool success)
        {
            isLevelActive = false;
            if (success)
            {
                Debug.Log("Уровень пройден!");
            }
            else
            {
                Debug.Log("Время вышло! Уровень провален.");
                signalBus.Fire(new LevelEndedSignal(1,false));
                signalBus.Fire(new UIStateChangedSignal(typeof(LoseWindow)));
            }
        }
    }
}