using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundMenu : MonoBehaviour
{
    private AudioManager sfxManager;

    [Header("Sliders")]
    [SerializeField] private Slider globalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header ("Toggle")]
    [SerializeField] private Toggle muteToggle;

    private void Awake()
    {
        //Initilize Sliders
        globalSlider.value = AudioSaver.globalVolume * globalSlider.maxValue / AudioSaver.GLOBAL_MAX;
        musicSlider.value = AudioSaver.musicVolume * musicSlider.maxValue;
        sfxSlider.value = AudioSaver.sfxVolume * sfxSlider.maxValue;

        //Initialize Mute Toggle
        muteToggle.isOn = AudioSaver.mute;
    }

    private void Start()
    {
        sfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();
    }

    /// <summary>
    /// Slider to change the global volume
    /// </summary>
    public void ChangeGlobalVolume(string sound)
    {
        Slider tmp = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();

        if (tmp != null)
        {
            AudioSaver.globalVolume = tmp.value * AudioSaver.GLOBAL_MAX / tmp.maxValue;

            if (sound != "NO")
                sfxManager.Play(sound);
        }

        AudioSaver.ApplyChanges();
    }

    /// <summary>
    /// Slider to change the Sfx volume
    /// </summary>
    public void ChangeSfxVolume(string sound)
    {
        Slider tmp = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();

        if (tmp != null)
        {
            AudioSaver.sfxVolume = tmp.value / tmp.maxValue;

            if (sound != "NO")
                sfxManager.Play(sound);
        }

        AudioSaver.ApplyChanges();
    }

    /// <summary>
    /// Slider to change the Music volume
    /// </summary>
    public void ChangeMusicVolume(string sound)
    {
        Slider tmp = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();

        if (tmp != null)
        {
            AudioSaver.musicVolume = tmp.value / tmp.maxValue;

            if (sound != "NO")
                sfxManager.Play(sound);
        }

        AudioSaver.ApplyChanges();
    }

    public void Mute(bool changeState)
    {
        SoundEvents.Instance.PressButton.Invoke();
        if (changeState) muteToggle.isOn = !muteToggle.isOn;
        AudioSaver.mute = muteToggle.isOn;
        AudioSaver.ApplyChanges();
    }
}
