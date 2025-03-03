namespace DI.SignalBus
{
    public class SettingsUpdatedSignal
    {
        public float MusicVolume { get; }
        public float SFXVolume { get; }
        public string Language { get; }

        public SettingsUpdatedSignal(float musicVolume, float sfxVolume, string language)
        {
            MusicVolume = musicVolume;
            SFXVolume = sfxVolume;
            Language = language;
        }
    }
}