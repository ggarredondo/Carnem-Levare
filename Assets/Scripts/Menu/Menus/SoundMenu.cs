using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundMenu : MonoBehaviour
{
    private AudioSaver audioMixer;
    private AudioManager sfxManager;

    [Header("Sliders")]
    [SerializeField] private Slider globalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header ("Toggle")]
    [SerializeField] private Toggle muteToggle;

    private void Awake()
    {
        sfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();
        audioMixer = GameObject.FindGameObjectWithTag("AUDIO").GetComponent<AudioSaver>();

        //Initilize Sliders
        globalSlider.value = audioMixer.globalVolume * globalSlider.maxValue / AudioSaver.GLOBAL_MAX;
        musicSlider.value = audioMixer.musicVolume * musicSlider.maxValue;
        sfxSlider.value = audioMixer.sfxVolume * sfxSlider.maxValue;

        //Initialize Mute Toggle
        muteToggle.isOn = audioMixer.mute;
    }

    /// <summary>
    /// Slider to change the global volume
    /// </summary>
    public void ChangeGlobalVolume(string sound)
    {
        Slider tmp = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();

        if (tmp != null)
        {
            audioMixer.globalVolume = tmp.value * AudioSaver.GLOBAL_MAX / tmp.maxValue;

            if (sound != "NO")
                sfxManager.Play(sound);
        }

        audioMixer.ApplyChanges();
    }

    /// <summary>
    /// Slider to change the Sfx volume
    /// </summary>
    public void ChangeSfxVolume(string sound)
    {
        Slider tmp = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();

        if (tmp != null)
        {
            audioMixer.sfxVolume = tmp.value / tmp.maxValue;

            if (sound != "NO")
                sfxManager.Play(sound);
        }

        audioMixer.ApplyChanges();
    }

    /// <summary>
    /// Slider to change the Music volume
    /// </summary>
    public void ChangeMusicVolume(string sound)
    {
        Slider tmp = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();

        if (tmp != null)
        {
            audioMixer.musicVolume = tmp.value / tmp.maxValue;

            if (sound != "NO")
                sfxManager.Play(sound);
        }

        audioMixer.ApplyChanges();
    }

    public void Mute()
    {
        muteToggle.isOn = !muteToggle.isOn;
        audioMixer.mute = muteToggle.isOn;
        audioMixer.ApplyChanges();
    }
}
