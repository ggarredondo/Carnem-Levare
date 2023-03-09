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
    private AudioManager sfxManager;

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

        PressButton += OnPressButton;
        SelectButton += OnSelectButton;
        PlayGame += OnPlayGame;
        PauseGame += OnPauseGame;
        BackMenu += OnBackMenu;
        ApplyRebind += OnApplyRebind;
    }

    public void StopSound(string sound)
    {
        sfxManager?.Stop(sound);
    }

    private void Start()
    {
        sfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();
    }

    private void OnPressButton()
    {
        sfxManager.Play("PressButton");
    }

    private void OnSelectButton()
    {
        sfxManager.Play("SelectButton");
    }

    private void OnPlayGame()
    {
        sfxManager.Play("PlayGame");
    }

    private void OnPauseGame()
    {
        sfxManager.Play("PauseGame");
    }

    private void OnBackMenu()
    {
        sfxManager.Play("BackMenu");
    }

    private void OnApplyRebind()
    {
        sfxManager.Play("ApplyRebind");
    }
}
