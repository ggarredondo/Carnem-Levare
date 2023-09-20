using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Entity { Player, Enemy }
public class AudioController : Singleton<AudioController>
{
    public SoundStructure uiSfxSounds;
    public SoundStructure gameSfxSounds;
    public SoundStructure musicSounds;

    private bool isPlayingSlider;

    public void InitializeSoundSources(List<SoundStructure> sounds)
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

    public void Walking(string sound)
    {
        gameSfxSounds.Play(sound);
    }

    private IEnumerator PlaySlider()
    {
        uiSfxSounds.Play("Slider");
        isPlayingSlider = true;
        yield return new WaitForSecondsRealtime(uiSfxSounds.Length("Slider") / 6);
        isPlayingSlider = false;
    }
}
