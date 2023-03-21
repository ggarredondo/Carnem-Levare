using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEvents : MonoBehaviour
{
    public static SoundEvents Instance { get; private set; }

    public Sounds uiSfxSounds;
    public Sounds gameSfxSounds;
    public Sounds musicSounds;

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

    public void StopSound(string sound) { uiSfxSounds?.Stop(sound); }

    public void PlayMusic(string sound) { musicSounds.StopAllSounds(); musicSounds?.Play(sound); }

    public void PlaySfx(string sound) { gameSfxSounds?.Play(sound); }

    public void PlaySfx(string sound, Entity actualSource) { gameSfxSounds?.Play(sound, (int) actualSource); }

    private void OnEnable()
    {
        PressButton += () => { uiSfxSounds.Play("PressButton"); };
        SelectButton += () => { uiSfxSounds.Play("SelectButton"); };
        PlayGame += () => { uiSfxSounds.Play("PlayGame"); };
        BackMenu += () => { uiSfxSounds.StopAllSounds(); uiSfxSounds.Play("BackMenu"); };
        ApplyRebind += () => { uiSfxSounds.Play("ApplyRebind"); };
        ExitLoading += () => { uiSfxSounds.Play("ExitLoading"); };
        MaskAlert += () => { uiSfxSounds.Play("MaskAlert"); };
        Slider += () => { if (!isPlayingSlider) StartCoroutine(PlaySlider()); };

        PauseGame += (bool enter) =>
        {
            if (enter) gameSfxSounds.PauseAllSounds();

            uiSfxSounds.Stop("PauseGame"); uiSfxSounds.Play("PauseGame");

            //if (!enter) gameSfxSounds.ResumeAllSounds();
        };

        Walking += (int foot, Entity actualSource) =>
        {
            if (foot == 0) gameSfxSounds.Play("Left_Foot", (int) actualSource);
            else gameSfxSounds.Play("Right_Foot", (int)actualSource);
        };
    }

    public IEnumerator PlaySlider()
    {
        uiSfxSounds.Play("Slider");
        isPlayingSlider = true;
        yield return new WaitForSecondsRealtime(uiSfxSounds.Length("Slider") / 6);
        isPlayingSlider = false;
    }
}
