using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sounds uiSfxSounds;
    public Sounds gameSfxSounds;
    public Sounds musicSounds;

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

    #region SpecialSounds

    public void PlayMusic(string name)
    {
        musicSounds.StopAllSounds();
        musicSounds.Play(name);
    }

    public void BackMenu()
    {
        uiSfxSounds.StopAllSounds(); 
        uiSfxSounds.Play("BackMenu");
    }

    public void Slider()
    {
        if (!isPlayingSlider) StartCoroutine(PlaySlider());
    }

    public void PauseGame(bool enter)
    {
        if (enter) gameSfxSounds.PauseAllSounds();

        uiSfxSounds.Stop("PauseGame"); 
        uiSfxSounds.Play("PauseGame");

        if (!enter) gameSfxSounds.ResumeAllSounds();
    }

    public void Walking(int foot, Entity actualSource)
    {
        if (foot == 0) gameSfxSounds.Play("Left_Foot", (int)actualSource);
        else gameSfxSounds.Play("Right_Foot", (int)actualSource);
    }

    private IEnumerator PlaySlider()
    {
        uiSfxSounds.Play("Slider");
        isPlayingSlider = true;
        yield return new WaitForSecondsRealtime(uiSfxSounds.Length("Slider") / 6);
        isPlayingSlider = false;
    }

    #endregion
}
