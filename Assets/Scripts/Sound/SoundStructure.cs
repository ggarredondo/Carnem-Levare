using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Scriptable Objects/SoundStructure")]
public class SoundStructure : ScriptableObject
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

                    if(group.speakers.GetLength(0) > 1)
                        SoundsTable.Add(group.ID[i] + "_" + s.name, s.source[i]);
                    else
                        SoundsTable.Add(s.name, s.source[i]);
                }
            }
        }
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

    public void PlayAtPoint(string name, Vector3 point)
    {
        AudioSource.PlayClipAtPoint(FindSound(name).clip, point);
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

    public void PauseAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            AudioSource s = (AudioSource)entry.Value;

            if (s.isPlaying)
            {
                s.Pause();
            }
        }
    }

    public void ResumeAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            AudioSource s = (AudioSource)entry.Value;
            s.UnPause();
        }
    }

    public void MuteAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            AudioSource s = (AudioSource)entry.Value;
            s.mute = true;
        }
    }

    public void UnMuteAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            AudioSource s = (AudioSource)entry.Value;
            s.mute = false;
        }
    }

    public void StopAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            AudioSource s = (AudioSource)entry.Value;
            s.Stop();
        }
    }

    public void ChangeVolume(float volume)
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            AudioSource s = (AudioSource)entry.Value;
            s.volume *= volume;
        }
    }

    private AudioSource FindSound(string name)
    {
        if (!SoundsTable.ContainsKey(name))
        {
            Debug.LogWarning("Sound: " + name + " doesn't exist");
            return null;
        }
        else
        {
            return (AudioSource) SoundsTable[name];
        }
    }
}
