using System;
using DI.SignalBus;
using UnityEditor;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using SettingsService = Services.SettingsService;

namespace UI.Windows
{
    public class SettingsWindow : UIWindow
    {
        private SignalBus signalBus;
        private SettingsService settingsService;
        
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Dropdown languageDropdown;
        [SerializeField] private Button closeButton;
        
        [Inject]
        private void Construct(SignalBus signalBus, SettingsService settingsService)
        {
            this.signalBus = signalBus;
            this.settingsService = settingsService;
        }

        private void Awake()
        {
            closeButton.onClick.AddListener(OnCloseButtonPressed);
            musicSlider.onValueChanged.AddListener(settingsService.SetMusicVolume);
            volumeSlider.onValueChanged.AddListener(settingsService.SetSFXVolume);
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            
            settingsService.InitializeUI(this);
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonPressed);
            musicSlider.onValueChanged.RemoveListener(settingsService.SetMusicVolume);
            volumeSlider.onValueChanged.RemoveListener(settingsService.SetSFXVolume);
            languageDropdown.onValueChanged.RemoveListener(OnLanguageChanged);
        }
        
        public void SetMusicVolume(float value)
        {
            musicSlider.value = value;
            Debug.Log($"Music Volume: {value}");
        }

        public void SetSFXVolume(float value)
        {
            volumeSlider.value = value;
        }

        public void SetLanguageDropdown(int index)
        {
            languageDropdown.value = index;
        }
        
        private void OnLanguageChanged(int index)
        {
            string selectedLanguage = languageDropdown.options[index].text;
            settingsService.SetLanguage(selectedLanguage);
        }
        
        private void OnCloseButtonPressed()
        {
            signalBus.Fire(new UIStateChangedSignal(typeof(MenuWindow)));
        }
    }
}