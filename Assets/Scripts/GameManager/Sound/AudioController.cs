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

    public static System.Action InitializeSound;

    public AudioController()
    {
        globalTable = new();
        groupConnection = new();
        groupConnection.Add(MAIN_GROUP_NAME, globalTable);
    }

    public void InitializeSoundSources(ref List<SoundStructure> soundStructures, ref List<Hashtable> soundTables)
    {
        for (int i = 0; i < soundStructures.Count; i++)
        {
            string structureName = soundStructures[i].GetStructureName();

            if (!groupConnection.ContainsKey(structureName))
                groupConnection.Add(structureName, new Hashtable());

            foreach (DictionaryEntry entry in soundTables[i])
            {
                globalTable.Add(entry.Key, entry.Value);
                groupConnection[structureName].Add(entry.Key, entry.Value);
            }
        }
    }

    public void DeleteSoundSources(ref List<SoundStructure> soundStructures, ref List<Hashtable> soundTables)
    {
        for (int i = 0; i < soundStructures.Count; i++)
        {
            string structureName = soundStructures[i].GetStructureName();

            foreach (DictionaryEntry entry in soundTables[i])
            {
                globalTable.Remove(entry.Key);
                groupConnection[structureName].Remove(entry.Key);
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
            ((AudioSource)entry.Value).UnPause();
    }

    public void MuteAllSounds(string mixerGroup = MAIN_GROUP_NAME)
    {
        foreach (DictionaryEntry entry in groupConnection[mixerGroup])
            ((AudioSource)entry.Value).mute = true;
    }

    public void UnMuteAllSounds(string mixerGroup = MAIN_GROUP_NAME)
    {
        foreach (DictionaryEntry entry in groupConnection[mixerGroup])
            ((AudioSource)entry.Value).mute = false;
    }

    public void StopAllSounds(string mixerGroup = MAIN_GROUP_NAME)
    {
        foreach (DictionaryEntry entry in groupConnection[mixerGroup])
            ((AudioSource)entry.Value).Stop();
    }

    private AudioSource FindSound(string name)
    {
        if (!globalTable.ContainsKey(name))
        {
            Debug.LogWarning("Sound: " + name + " doesn't exist");
            return null;
        }
        else
            return (AudioSource)globalTable[name];
    }
}
