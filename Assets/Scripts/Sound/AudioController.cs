using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AudioController
{
    private const string MAIN_GROUP_NAME = "main";

    private bool isPlayingSlider;
    private Hashtable globalTable;
    private Dictionary<string, Hashtable> groupConnection;

    public void InitializeSoundSources(List<SoundStructure> soundStructure)
    {
        globalTable = new();
        groupConnection = new();
        groupConnection.Add(MAIN_GROUP_NAME, globalTable);

        for (int i = 0; i < soundStructure.Count; i++)
        {
            for (int j = 0; j < soundStructure[i].SoundGroups.GetLength(0); j++)
            {
                soundStructure[i].SoundGroups[j].speakers = new GameObject[soundStructure[i].SoundGroups[j].speakersTag.GetLength(0)];

                for (int k = 0; k < soundStructure[i].SoundGroups[j].speakersTag.GetLength(0); k++)
                {
                    GameObject speaker = GameObject.FindGameObjectWithTag(soundStructure[i].SoundGroups[j].speakersTag[k]);
                    soundStructure[i].SoundGroups[j].speakers[k] = speaker;
                }
            }

            soundStructure[i].Initialize();

            Hashtable soundsTable = soundStructure[i].GetSoundsTable();

            groupConnection.Add(soundStructure[i].GetGroupName(), soundsTable);

            foreach (DictionaryEntry entry in soundsTable)
            {
                globalTable.Add(entry.Key, entry.Value);
            }
        }
    }

    public void PlayMusic(string name)
    {
        StopAllSounds("Music");
        Play(name);
    }

    public void BackMenu()
    {
        StopAllSounds("Ui_Sfx"); 
        Play("BackMenu");
    }

    public void Slider()
    {
        if (!isPlayingSlider) PlaySlider();
    }

    public void PauseGame(bool enter)
    {
        if (enter) PauseAllSounds("Game_Sfx");

        Stop("PauseGame"); 
        Play("PauseGame");

        if (!enter) ResumeAllSounds("Game_Sfx");
    }

    private async void PlaySlider()
    {
        Play("Slider");
        isPlayingSlider = true;
        await Task.Delay(System.TimeSpan.FromSeconds(Length("Slider") / 6));
        isPlayingSlider = false;
    }

    public void ChangePitch(string name, float pitch)
    {
        FindSound(name).pitch = pitch;
    }

    public float Length(string name)
    {
        return FindSound(name).clip.length;
    }

    public void Play(string name)
    {
        FindSound(name)?.Play();
    }

    public bool IsPlaying(string name)
    {
        return FindSound(name).isPlaying;
    }

    public void Stop(string name)
    {
        FindSound(name)?.Stop();
    }

    public void Pause(string name)
    {
        FindSound(name)?.Pause();
    }

    public void PauseAllSounds(string mixerGroup = MAIN_GROUP_NAME)
    {
        foreach (DictionaryEntry entry in groupConnection[mixerGroup])
        {
            AudioSource s = (AudioSource)entry.Value;

            if (s.isPlaying)
            {
                s.Pause();
            }
        }
    }

    public void ResumeAllSounds(string mixerGroup = MAIN_GROUP_NAME)
    {
        foreach (DictionaryEntry entry in groupConnection[mixerGroup])
        {
            AudioSource s = (AudioSource)entry.Value;
            s.UnPause();
        }
    }

    public void MuteAllSounds(string mixerGroup = MAIN_GROUP_NAME)
    {
        foreach (DictionaryEntry entry in groupConnection[mixerGroup])
        {
            AudioSource s = (AudioSource)entry.Value;
            s.mute = true;
        }
    }

    public void UnMuteAllSounds(string mixerGroup = MAIN_GROUP_NAME)
    {
        foreach (DictionaryEntry entry in groupConnection[mixerGroup])
        {
            AudioSource s = (AudioSource)entry.Value;
            s.mute = false;
        }
    }

    public void StopAllSounds(string mixerGroup = MAIN_GROUP_NAME)
    {
        foreach (DictionaryEntry entry in groupConnection[mixerGroup])
        {
            AudioSource s = (AudioSource)entry.Value;
            s.Stop();
        }
    }

    private AudioSource FindSound(string name)
    {
        if (!globalTable.ContainsKey(name))
        {
            Debug.LogWarning("Sound: " + name + " doesn't exist");
            return null;
        }
        else
        {
            return (AudioSource)globalTable[name];
        }
    }
}
