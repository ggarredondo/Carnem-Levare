using UnityEngine;
using UnityEngine.UI;

public class SoundMenu : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider globalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header ("Toggle")]
    [SerializeField] private Toggle muteToggle;

    private void Start()
    {
        globalSlider.value = DataSaver.options.masterVolume;
        musicSlider.value = DataSaver.options.musicVolume;
        sfxSlider.value = DataSaver.options.sfxVolume;

        muteToggle.isOn = DataSaver.options.mute;
    }

    public void ChangeGlobalVolume()
    {
        DataSaver.options.masterVolume = globalSlider.value;

        if (DataSaver.options.musicVolume < 0.1)
            AudioManager.Instance.Slider();

        OptionsApplier.apply.Invoke();
    }

    public void ChangeSfxVolume()
    {
        DataSaver.options.sfxVolume = sfxSlider.value;
        AudioManager.Instance.Slider();

        OptionsApplier.apply.Invoke();
    }

    public void ChangeMusicVolume()
    {
        DataSaver.options.musicVolume = musicSlider.value;

        OptionsApplier.apply.Invoke();
    }

    public void Mute(bool changeState)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        if (changeState) muteToggle.isOn = !muteToggle.isOn;
        DataSaver.options.mute = muteToggle.isOn;
        OptionsApplier.apply.Invoke();
    }
}
