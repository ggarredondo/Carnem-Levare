using UnityEngine;
using UnityEngine.UI;

public class SoundMenu : AbstractMenu
{
    [Header("UI Elements")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private Button muteButton;

    protected override void Configure()
    {
        masterSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSfxVolume);
        muteToggle.onValueChanged.AddListener(Mute);

        muteButton.onClick.AddListener(() => muteToggle.isOn = !muteToggle.isOn);

        masterSlider.value = DataSaver.Options.masterVolume;
        musicSlider.value = DataSaver.Options.musicVolume;
        sfxSlider.value = DataSaver.Options.sfxVolume;
        muteToggle.isOn = DataSaver.Options.mute;
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
