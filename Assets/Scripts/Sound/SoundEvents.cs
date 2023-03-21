using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEvents : MonoBehaviour
{

    public static SoundEvents Instance { get; private set; }

    public AudioMixerGroup audioMixerGroup;

    public delegate void SoundEventHandler();
    public delegate void PauseMenuHandler(bool enter);
    public delegate void WalkingHandler(int foot, Entity actualSource);

    public SoundEventHandler PressButton;
    public SoundEventHandler SelectButton;
    public SoundEventHandler PlayGame;
    public SoundEventHandler BackMenu;
    public SoundEventHandler ApplyRebind;
    public SoundEventHandler ExitLoading;
    public SoundEventHandler MaskAlert;
    public SoundEventHandler Slider;

    public PauseMenuHandler PauseGame;
    public WalkingHandler Walking;

    private AudioManager uiSfxManager, gameSfxManager;
    private AudioManager musicManager;
    private bool isPlayingSlider;

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

    public void PlaySfx(string sound, Entity actualSource) { gameSfxManager?.Play(sound, (int) actualSource); }

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
        Slider += () => { if (!isPlayingSlider) StartCoroutine(PlaySlider()); };

        PauseGame += (bool enter) =>
        {
            if (enter) uiSfxManager.PauseAllSounds();

            uiSfxManager.Stop("PauseGame"); uiSfxManager.Play("PauseGame");

            if (!enter) uiSfxManager.ResumeAllSounds();
        };

        Walking += (int foot, Entity actualSource) =>
        {
            if (foot == 0) gameSfxManager.Play("Left_Foot", (int) actualSource);
            else gameSfxManager.Play("Right_Foot", (int)actualSource);
        };
    }

    public IEnumerator PlaySlider()
    {
        uiSfxManager.Play("Slider");
        isPlayingSlider = true;
        yield return new WaitForSecondsRealtime(uiSfxManager.Length("Slider") / 6);
        isPlayingSlider = false;
    }
}
