using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Scriptable Objects/Sounds")]
public class Sounds : ScriptableObject
{
    [Header("AudioMixer")]
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [Header("3D Sound")]
    [SerializeField] private bool threeD;

    [ConditionalField("threeD")] [Range(0, 5)] [SerializeField] private float dopplerLevel = 1f;
    [ConditionalField("threeD")] [Range(0, 360)] [SerializeField] private float spread = 0f;
    [ConditionalField("threeD")] [SerializeField] private AudioRolloffMode volumeRolloff = AudioRolloffMode.Logarithmic;
    [ConditionalField("threeD")] [SerializeField] private float minDistance = 1f;
    [ConditionalField("threeD")] [SerializeField] private float maxDistance = 500f;
    [ConditionalField("threeD")] [SerializeField] private AnimationCurve customRollofCurve;

    [Header("Manager Sounds")]
    [SerializeField] private SoundGroup[] soundGroups;

    private int spatialBlend;
    private Hashtable SoundsTable;

    public SoundGroup[] SoundGroups { get { return soundGroups; } }

    public void Initialize()
    {
        spatialBlend = threeD ? 1 : 0;
        SoundsTable = new Hashtable();

        foreach (SoundGroup group in soundGroups)
        {
            foreach (Sound s in group.sounds)
            {
                s.source = new AudioSource[group.speakers.GetLength(0)];

                for (int i = 0; i < group.speakers.GetLength(0); i++)
                {
                    s.source[i] = group.speakers[i].AddComponent<AudioSource>();
                    s.source[i].clip = s.clip;
                    s.source[i].volume = s.volume;
                    s.source[i].pitch = s.pitch;
                    s.source[i].loop = s.loop;
                    s.source[i].spatialBlend = spatialBlend;

                    if (threeD)
                    {
                        s.source[i].dopplerLevel = dopplerLevel;
                        s.source[i].spread = spread;
                        s.source[i].rolloffMode = volumeRolloff;
                        s.source[i].minDistance = minDistance;
                        s.source[i].maxDistance = maxDistance;
                        s.source[i].SetCustomCurve(AudioSourceCurveType.CustomRolloff, customRollofCurve);
                    }

                    s.source[i].outputAudioMixerGroup = audioMixerGroup;
                }

                SoundsTable.Add(s.name, s);
            }
        }
    }

    public void ChangePitch(string name, float pitch, int actualSource = 0)
    {
        FindSound(name).source[actualSource].pitch = pitch;
    }

    public float Length(string name)
    {
        return FindSound(name).clip.length;
    }

    public void Play(string name, int actualSource = 0)
    {
        FindSound(name)?.source[actualSource].Play();
    }

    public void PlayAtPoint(string name, Vector3 point)
    {
        AudioSource.PlayClipAtPoint(FindSound(name).clip, point);
    }

    public bool IsPlaying(string name, int actualSource = 0)
    {
        return FindSound(name).source[actualSource].isPlaying;
    }

    public void Stop(string name, int actualSource = 0)
    {
        FindSound(name)?.source[actualSource].Stop();
    }

    public void Pause(string name, int actualSource = 0)
    {
        FindSound(name)?.source[actualSource].Pause();
    }

    public void PauseAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            Sound s = (Sound)entry.Value;
            for (int i = 0; i < s.source.GetLength(0); i++)
            {
                if (s.source[i].isPlaying)
                {
                    s.source[i].Pause();
                }
            }
        }
    }

    public void ResumeAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            Sound s = (Sound)entry.Value;
            for (int i = 0; i < s.source.GetLength(0); i++)
            {
                s.source[i].UnPause();
            }
        }
    }

    public void MuteAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            Sound s = (Sound)entry.Value;
            for (int i = 0; i < s.source.GetLength(0); i++)
                s.source[i].mute = true;
        }
    }

    public void UnMuteAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            Sound s = (Sound)entry.Value;
            for (int i = 0; i < s.source.GetLength(0); i++)
                s.source[i].mute = false;
        }
    }

    public void StopAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            Sound s = (Sound)entry.Value;
            for (int i = 0; i < s.source.GetLength(0); i++)
                s.source[i].Stop();
        }
    }

    public void ChangeVolume(float volume)
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            Sound s = (Sound)entry.Value;
            for (int i = 0; i < s.source.GetLength(0); i++)
                s.source[i].volume = s.volume * volume;
        }
    }

    private Sound FindSound(string name)
    {
        if (!SoundsTable.ContainsKey(name))
        {
            Debug.LogWarning("Sound: " + name + " doesn't exist");
            return null;
        }
        else
        {
            return (Sound) SoundsTable[name];
        }
    }
}
