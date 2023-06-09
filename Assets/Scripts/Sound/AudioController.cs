using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Entity { Player, Enemy }
public class AudioController : Singleton<AudioController>
{
    public Sounds uiSfxSounds;
    public Sounds gameSfxSounds;
    public Sounds musicSounds;

    private bool isPlayingSlider;

    public void InitializeSoundSources(List<Sounds> sounds)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            for (int j = 0; j < sounds[i].SoundGroups.GetLength(0); j++)
            {
                sounds[i].SoundGroups[j].speakers = new GameObject[sounds[i].SoundGroups[j].speakersTag.GetLength(0)];

                for (int k = 0; k < sounds[i].SoundGroups[j].speakersTag.GetLength(0); k++)
                {
                    GameObject speaker = GameObject.FindGameObjectWithTag(sounds[i].SoundGroups[j].speakersTag[k]);
                    sounds[i].SoundGroups[j].speakers[k] = speaker;
                }
            }

            sounds[i].Initialize();
        }
    }

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
}
