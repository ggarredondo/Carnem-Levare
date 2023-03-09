using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvents : MonoBehaviour
{
    public static SoundEvents Instance { get; private set; }

    public delegate void SoundEventHandler();
    public SoundEventHandler PressButton;
    public SoundEventHandler SelectButton;
    public SoundEventHandler PlayGame;
    public SoundEventHandler PauseGame;
    public SoundEventHandler BackMenu;
    public SoundEventHandler ApplyRebind;
    public SoundEventHandler ExitLoading;
    public SoundEventHandler MaskAlert;

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

    private void Start()
    {
        sfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();
        musicManager = GameObject.FindGameObjectWithTag("MUSIC").GetComponent<AudioManager>();

        PressButton += () => { sfxManager.Play("PressButton"); };
        SelectButton += () => { sfxManager.Play("SelectButton"); };
        PlayGame += () => { sfxManager.Play("PlayGame"); };
        PauseGame += () => { sfxManager.Play("PauseGame"); sfxManager.Play("PauseGame"); };
        BackMenu += () => { sfxManager.Play("BackMenu"); };
        ApplyRebind += () => { sfxManager.Play("ApplyRebind"); };
        ExitLoading += () => { sfxManager.Play("ExitLoading"); };
        MaskAlert += () => { sfxManager.Play("MaskAlert"); };
    }
}
