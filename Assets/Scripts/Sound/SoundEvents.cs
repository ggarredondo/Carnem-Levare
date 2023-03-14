using UnityEngine;
using UnityEngine.Audio;

public class SoundEvents : MonoBehaviour
{

    public static SoundEvents Instance { get; private set; }

    public AudioMixerGroup audioMixerGroup;
    public delegate void SoundEventHandler();
    public delegate void PauseMenuHandler(bool enter);

    public SoundEventHandler PressButton;
    public SoundEventHandler SelectButton;
    public SoundEventHandler PlayGame;
    public SoundEventHandler BackMenu;
    public SoundEventHandler ApplyRebind;
    public SoundEventHandler ExitLoading;
    public SoundEventHandler MaskAlert;
    public SoundEventHandler Slider;

    public PauseMenuHandler PauseGame;

    private AudioManager sfxManager;
    private AudioManager musicManager;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void StopSound(string sound) { sfxManager?.Stop(sound); }

    public void PlayMusic(string sound) { musicManager.StopAllSounds(); musicManager?.Play(sound); }

    public void PlaySfx(string sound) { sfxManager?.Play(sound); }

    private void OnEnable()
    {
        sfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();
        musicManager = GameObject.FindGameObjectWithTag("MUSIC").GetComponent<AudioManager>();

        PressButton += () => { sfxManager.Play("PressButton"); };
        SelectButton += () => { sfxManager.Play("SelectButton"); };
        PlayGame += () => { sfxManager.Play("PlayGame"); };
        BackMenu += () => { sfxManager.StopAllSounds(); sfxManager.Play("BackMenu"); };
        ApplyRebind += () => { sfxManager.Play("ApplyRebind"); };
        ExitLoading += () => { sfxManager.Play("ExitLoading"); };
        MaskAlert += () => { sfxManager.Play("MaskAlert"); };
        Slider += () => { sfxManager.Play("Slider"); };

        PauseGame += (bool enter) =>
        {
            if (enter) sfxManager.PauseAllSounds();

            sfxManager.Stop("PauseGame"); sfxManager.Play("PauseGame");

            if (!enter) sfxManager.ResumeAllSounds();
        };
    }
}
