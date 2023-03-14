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

    private AudioManager uiSfxManager, gameSfxManager;
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

    public void StopSound(string sound) { uiSfxManager?.Stop(sound); }

    public void PlayMusic(string sound) { musicManager.StopAllSounds(); musicManager?.Play(sound); }

    public void PlaySfx(string sound) { gameSfxManager?.Play(sound); }

    private void OnEnable()
    {
        uiSfxManager = GameObject.FindGameObjectWithTag("SFX").transform.GetChild(0).GetComponent<AudioManager>();
        gameSfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();
        musicManager = GameObject.FindGameObjectWithTag("MUSIC").GetComponent<AudioManager>();

        PressButton += () => { uiSfxManager.Play("PressButton"); };
        SelectButton += () => { uiSfxManager.Play("SelectButton"); };
        PlayGame += () => { uiSfxManager.Play("PlayGame"); };
        BackMenu += () => { uiSfxManager.StopAllSounds(); uiSfxManager.Play("BackMenu"); };
        ApplyRebind += () => { uiSfxManager.Play("ApplyRebind"); };
        ExitLoading += () => { uiSfxManager.Play("ExitLoading"); };
        MaskAlert += () => { uiSfxManager.Play("MaskAlert"); };
        Slider += () => { uiSfxManager.Play("Slider"); };

        PauseGame += (bool enter) =>
        {
            if (enter) uiSfxManager.PauseAllSounds();

            uiSfxManager.Stop("PauseGame"); uiSfxManager.Play("PauseGame");

            if (!enter) uiSfxManager.ResumeAllSounds();
        };
    }
}
