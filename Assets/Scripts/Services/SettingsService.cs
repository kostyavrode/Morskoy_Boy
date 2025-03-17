using DI.SignalBus;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Services
{
    public class SettingsService
    {
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SFXVolume";
        private const string LANGUAGE_KEY = "Language";
        
        private AudioSource musicSource;
        private AudioSource sfxSource;
        private SignalBus signalBus;    
        private SettingsWindow settingsWindow;

        private float musicVolume;
        private float sfxVolume;
        private string language;
        
        public float GetMusicVolume() => musicVolume;
        public float GetSFXVolume() => sfxVolume;
        public string GetLanguage() => language;
  
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
            
            musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
            sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
            language = PlayerPrefs.GetString(LANGUAGE_KEY, "en");
            
            signalBus.Fire(new SettingsUpdatedSignal(musicVolume, sfxVolume, language));
        }
        
        public void Initialize(AudioSource music, AudioSource sfx)
        {
            musicSource = music;
            sfxSource = sfx;

            ApplySettings();
        }
        
        public void InitializeUI(UI.Windows.SettingsWindow window)
        {
            settingsWindow = window;
            
            settingsWindow.SetMusicVolume(musicVolume);
            settingsWindow.SetSFXVolume(sfxVolume);
            settingsWindow.SetLanguageDropdown(GetLanguageIndex(language));
        }

        private void ApplySettings()
        {
            if (musicSource) musicSource.volume = musicVolume;
            if (sfxSource) sfxSource.volume = sfxVolume;

            SetLanguage(language);
        }
        
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
            PlayerPrefs.Save();
            ApplySettings();
            signalBus.Fire(new SettingsUpdatedSignal(musicVolume, sfxVolume, language));
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
            PlayerPrefs.Save();
            ApplySettings();
            signalBus.Fire(new SettingsUpdatedSignal(musicVolume, sfxVolume, language));
        }

        public void SetLanguage(string language)
        {
            this.language = language;
            PlayerPrefs.SetString(LANGUAGE_KEY, language);
            PlayerPrefs.Save();
            
            signalBus.Fire(new SettingsUpdatedSignal(musicVolume, sfxVolume, language));
        }
        
        private int GetLanguageIndex(string language)
        {
            return language == "en" ? 0 : 1;
        }
    }
}