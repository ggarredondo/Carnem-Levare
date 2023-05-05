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

    #region Initialization

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

    #endregion

    #region Individual_Utilities

    /// <summary>
    /// Change the pitch of any sound
    /// </summary>
    /// <param name="name">Sound name</param>
    /// <param name="pitch">Pitch we want to change to</param>
    public void ChangePitch(string name, float pitch, int actualSource = 0)
    {
        FindSound(name).source[actualSource].pitch = pitch;
    }

    /// <summary>
    /// Know the duration of a sound
    /// </summary>
    /// <param name="name">Sound name</param>
    /// <returns></returns>
    public float Length(string name)
    {
        return FindSound(name).clip.length;
    }

    /// <summary>
    /// Play a sound
    /// </summary>
    /// <param name="name">Sound name</param>
    public void Play(string name, int actualSource = 0)
    {
        FindSound(name)?.source[actualSource].Play();
    }

    /// <summary>
    /// Play a sound at point in 3D
    /// </summary>
    /// <param name="name">Sound name</param>
    /// <param name="point">Point in space</param>
    public void PlayAtPoint(string name, Vector3 point)
    {
        AudioSource.PlayClipAtPoint(FindSound(name).clip, point);
    }

    /// <summary>
    /// Know if a sound is playing
    /// </summary>
    /// <param name="name">Sound name</param>
    /// <returns></returns>
    public bool IsPlaying(string name, int actualSource = 0)
    {
        return FindSound(name).source[actualSource].isPlaying;
    }

    /// <summary>
    /// Stop a sound
    /// </summary>
    /// <param name="name">Sound name</param>
    public void Stop(string name, int actualSource = 0)
    {
        FindSound(name)?.source[actualSource].Stop();
    }

    /// <summary>
    /// Pause a sound
    /// </summary>
    /// <param name="name">Sound name</param>
    public void Pause(string name, int actualSource = 0)
    {
        FindSound(name)?.source[actualSource].Pause();
    }

    #endregion

    #region All_Sounds_Utilities

    /// <summary>
    /// Pause all the sounds included in audioManager
    /// </summary>
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

    /// <summary>
    /// Play all the sounds that have been paused
    /// </summary>
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

    /// <summary>
    /// Play all the sounds that have been paused
    /// </summary>
    public void MuteAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            Sound s = (Sound)entry.Value;
            for (int i = 0; i < s.source.GetLength(0); i++)
                s.source[i].mute = true;
        }
    }

    /// <summary>
    /// Play all the sounds that have been paused
    /// </summary>
    public void UnMuteAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            Sound s = (Sound)entry.Value;
            for (int i = 0; i < s.source.GetLength(0); i++)
                s.source[i].mute = false;
        }
    }

    /// <summary>
    /// Stop all the sounds included in audiomanager
    /// </summary>
    public void StopAllSounds()
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            Sound s = (Sound)entry.Value;
            for (int i = 0; i < s.source.GetLength(0); i++)
                s.source[i].Stop();
        }
    }

    /// <summary>
    /// Change the volume of all the AudioManager sounds
    /// </summary>
    /// <param name="volume">The percentage of volume that we want to apply</param>
    public void ChangeVolume(float volume)
    {
        foreach (DictionaryEntry entry in SoundsTable)
        {
            Sound s = (Sound)entry.Value;
            for (int i = 0; i < s.source.GetLength(0); i++)
                s.source[i].volume = s.volume * volume;
        }
    }

    #endregion

    #region Private

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

    #endregion
}
