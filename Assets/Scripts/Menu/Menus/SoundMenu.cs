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

    private void Awake()
    {
        //Initilize Sliders
        globalSlider.value = AudioSaver.Instance.globalVolume;
        musicSlider.value = AudioSaver.Instance.musicVolume;
        sfxSlider.value = AudioSaver.Instance.sfxVolume;

        //Initialize Mute Toggle
        muteToggle.isOn = AudioSaver.Instance.mute;
    }

    /// <summary>
    /// Slider to change the global volume
    /// </summary>
    public void ChangeGlobalVolume()
    {
        if (globalSlider != null)
        {
            AudioSaver.Instance.globalVolume = globalSlider.value;

            if (AudioSaver.Instance.musicVolume < 0.1)
                AudioManager.Instance.Slider();
        }

        AudioSaver.Instance.ApplyChanges();
    }

    /// <summary>
    /// Slider to change the Sfx volume
    /// </summary>
    public void ChangeSfxVolume()
    {
        if (sfxSlider != null)
        {
            AudioSaver.Instance.sfxVolume = sfxSlider.value;
            AudioManager.Instance.Slider();
        }

        AudioSaver.Instance.ApplyChanges();
    }

    /// <summary>
    /// Slider to change the Music volume
    /// </summary>
    public void ChangeMusicVolume()
    {
        if (musicSlider != null)
        {
            AudioSaver.Instance.musicVolume = musicSlider.value;
        }

        AudioSaver.Instance.ApplyChanges();
    }

    public void Mute(bool changeState)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        if (changeState) muteToggle.isOn = !muteToggle.isOn;
        AudioSaver.Instance.mute = muteToggle.isOn;
        AudioSaver.Instance.ApplyChanges();
    }
}
