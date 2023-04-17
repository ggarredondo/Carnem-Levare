using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundMenu : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider globalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header ("Toggle")]
    [SerializeField] private Toggle muteToggle;

    private IOptionsApplier applier;

    private void Awake()
    {
        applier = new AudioApplier(audioMixer);
    }

    private void Start()
    {
        applier.ApplyChanges();

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

        applier.ApplyChanges();
    }

    public void ChangeSfxVolume()
    {
        DataSaver.options.sfxVolume = sfxSlider.value;
        AudioManager.Instance.Slider();

        applier.ApplyChanges();
    }

    public void ChangeMusicVolume()
    {
        DataSaver.options.musicVolume = musicSlider.value;

        applier.ApplyChanges();
    }

    public void Mute(bool changeState)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        if (changeState) muteToggle.isOn = !muteToggle.isOn;
        DataSaver.options.mute = muteToggle.isOn;
        applier.ApplyChanges();
    }
}
