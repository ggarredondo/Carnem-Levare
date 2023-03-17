using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("AudioMixer")]
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [Header("3D Sound")]
    [SerializeField] private bool threeD;
    [NonSerialized] public GameObject[] speakers;

    // Define the 3D sound settings
    [ConditionalField("threeD")] [Range(0,5)]    [SerializeField] private float dopplerLevel = 1f;
    [ConditionalField("threeD")] [Range(0, 360)] [SerializeField] private float spread = 0f;
    [ConditionalField("threeD")] [SerializeField] private AudioRolloffMode volumeRolloff = AudioRolloffMode.Logarithmic;
    [ConditionalField("threeD")] [SerializeField] private float minDistance = 1f;
    [ConditionalField("threeD")] [SerializeField] private float maxDistance = 500f;
    [ConditionalField("threeD")] [SerializeField] private AnimationCurve customRollofCurve;

    [Header("Manager Sounds")]
    [SerializeField] private Sound[] sounds;

    private List<Sound> currentSounds = new();
    private bool notActive;
    private int spatialBlend;

    void Awake()
    {
        spatialBlend = threeD ? 1 : 0;

        if (!threeD)
        {
            speakers = new GameObject[1];
            speakers[0] = gameObject;
        }

        Initialize();
    }

    public bool NotActive { set { notActive = value; } }

    public void Initialize()
    {
        if (speakers?.GetLength(0) > 0)
        {
            notActive = false;
            foreach (Sound s in sounds)
            {
                s.source = new AudioSource[speakers.GetLength(0)];

                for (int i = 0; i < speakers.GetLength(0); i++)
                {
                    s.source[i] = speakers[i].AddComponent<AudioSource>();
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
            }
        }
        else notActive = true;
    }

    /// <summary>
    /// Change the pitch of any sound
    /// </summary>
    /// <param name="name">Sound name</param>
    /// <param name="pitch">Pitch we want to change to</param>
    public void ChangePitch(string name, float pitch, int actualSource = 0)
    {
        if (!notActive)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not exist");
            }

            s.source[actualSource].pitch = pitch;
        }
    }

    /// <summary>
    /// Know the duration of a sound
    /// </summary>
    /// <param name="name">Sound name</param>
    /// <returns></returns>
    public float Lenght(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not exist");
        }

        return s.clip.length;
    }

    /// <summary>
    /// Play a sound
    /// </summary>
    /// <param name="name">Sound name</param>
    public void Play(string name, int actualSource = 0)
    {
        if (!notActive)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " doesn't exist");
                return;
            }

            s.source[actualSource].Play();
        }
    }

    /// <summary>
    /// Stop a sound
    /// </summary>
    /// <param name="name">Sound name</param>
    public void Stop(string name, int actualSource = 0)
    {
        if (!notActive)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not exist");
                return;
            }

            s.source[actualSource].Stop();
        }
    }

    /// <summary>
    /// Pause a sound
    /// </summary>
    /// <param name="name">Sound name</param>
    public void Pause(string name, int actualSource = 0)
    {
        if (!notActive)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not exist");
                return;
            }

            s.source[actualSource].Pause();
        }
    }

    /// <summary>
    /// Pause all the sounds included in audioManager
    /// </summary>
    public void PauseAllSounds()
    {
        if (!notActive)
        {
            foreach (Sound sound in sounds)
            {
                for (int i = 0; i < speakers.GetLength(0); i++)
                {
                    if (sound.source[i].isPlaying)
                    {
                        sound.source[i].Pause();
                        currentSounds.Add(sound);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Play all the sounds that have been paused
    /// </summary>
    public void ResumeAllSounds()
    {
        if (!notActive)
        {
            foreach (Sound sound in currentSounds)
            {
                for (int i = 0; i < speakers.GetLength(0); i++)
                    sound.source[i].UnPause();
            }
        }
    }

    /// <summary>
    /// Play all the sounds that have been paused
    /// </summary>
    public void MuteAllSounds()
    {
        if (!notActive)
        {
            foreach (Sound sound in sounds)
            {
                for (int i = 0; i < speakers.GetLength(0); i++)
                    sound.source[i].mute = true;
            }
        }
    }

    /// <summary>
    /// Play all the sounds that have been paused
    /// </summary>
    public void UnMuteAllSounds()
    {
        if (!notActive)
        {
            foreach (Sound sound in sounds)
            {
                for (int i = 0; i < speakers.GetLength(0); i++)
                    sound.source[i].mute = false;
            }
        }
    }

    /// <summary>
    /// Stop all the sounds included in audiomanager
    /// </summary>
    public void StopAllSounds()
    {
        if (!notActive)
        {
            foreach (Sound sound in sounds)
            {
                for (int i = 0; i < speakers.GetLength(0); i++)
                    sound.source[i].Stop();
            }
        }
    }

    /// <summary>
    /// Play a sound at point in 3D
    /// </summary>
    /// <param name="name">Sound name</param>
    /// <param name="point">Point in space</param>
    public void PlayAtPoint(string name, Vector3 point)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not exist");
            return;
        }

        AudioSource.PlayClipAtPoint(s.clip, point);
    }

    /// <summary>
    /// Change the volume of all the AudioManager sounds
    /// </summary>
    /// <param name="volume">The percentage of volume that we want to apply</param>
    public void ChangeVolume(float volume)
    {
        if (!notActive)
        {
            foreach (Sound s in sounds)
            {
                for (int i = 0; i < speakers.GetLength(0); i++)
                    s.source[i].volume = s.volume * volume;
            }
        }
    }


    /// <summary>
    /// Know if a sound is playing
    /// </summary>
    /// <param name="name">Sound name</param>
    /// <returns></returns>
    public bool IsPlaying(string name, int actualSource = 0)
    {
        if (!notActive)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not exist");
                return false;
            }

            return s.source[actualSource].isPlaying;
        }
        else return false;
    }
}
