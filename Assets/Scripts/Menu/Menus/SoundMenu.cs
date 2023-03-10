using UnityEngine;
using UnityEngine.EventSystems;
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
        Slider tmp = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();

        if (tmp != null)
        {
            AudioSaver.Instance.globalVolume = tmp.value;

            if (AudioSaver.Instance.musicVolume == 0)
                SoundEvents.Instance.Slider.Invoke();
        }

        AudioSaver.Instance.ApplyChanges();
    }

    /// <summary>
    /// Slider to change the Sfx volume
    /// </summary>
    public void ChangeSfxVolume()
    {
        Slider tmp = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();

        if (tmp != null)
        {
            AudioSaver.Instance.sfxVolume = tmp.value;
            SoundEvents.Instance.Slider.Invoke();
        }

        AudioSaver.Instance.ApplyChanges();
    }

    /// <summary>
    /// Slider to change the Music volume
    /// </summary>
    public void ChangeMusicVolume()
    {
        Slider tmp = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();

        if (tmp != null)
        {
            AudioSaver.Instance.musicVolume = tmp.value;
        }

        AudioSaver.Instance.ApplyChanges();
    }

    public void Mute(bool changeState)
    {
        SoundEvents.Instance.PressButton.Invoke();
        if (changeState) muteToggle.isOn = !muteToggle.isOn;
        AudioSaver.Instance.mute = muteToggle.isOn;
        AudioSaver.Instance.ApplyChanges();
    }
}
