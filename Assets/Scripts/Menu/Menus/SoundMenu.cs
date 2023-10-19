using UnityEngine;

public class SoundMenu : AbstractMenu
{
    [Header("UI Elements")]
    [SerializeField] private MySlider master;
    [SerializeField] private MySlider music;
    [SerializeField] private MySlider sfx;
    [SerializeField] private MyToggle mute;

    protected override void Configure()
    {
        master.Value = DataSaver.Options.masterVolume;
        music.Value = DataSaver.Options.musicVolume;
        sfx.Value = DataSaver.Options.sfxVolume;
        mute.Value = DataSaver.Options.mute;
    }

    public void ChangeMasterVolume(float value)
    {
        Slider(ref DataSaver.Options.masterVolume, value, DataSaver.Options.musicVolume < 0.1);
    }

    public void ChangeMusicVolume(float value)
    {
        Slider(ref DataSaver.Options.musicVolume, value, false);
    }

    public void ChangeSfxVolume(float value)
    {
        Slider(ref DataSaver.Options.sfxVolume, value, true);
    }

    public void Mute(bool value)
    {
        Toggle(ref DataSaver.Options.mute, value);
    }
}
