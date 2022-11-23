using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private List<Sound> currentSounds = new List<Sound>();

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }

    /// <summary>
    /// Change the pitch of any sound
    /// </summary>
    /// <param name="name">Sound name</param>
    /// <param name="pitch">Pitch we want to change to</param>
    public void ChangePitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not exist");
        }

        s.source.pitch = pitch;
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
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not exist");
            return;
        }

        s.source.Play();
    }

    /// <summary>
    /// Stop a sound
    /// </summary>
    /// <param name="name">Sound name</param>
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not exist");
            return;
        }

        s.source.Stop();
    }

    /// <summary>
    /// Pause a sound
    /// </summary>
    /// <param name="name">Sound name</param>
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not exist");
            return;
        }

        s.source.Pause();
    }

    /// <summary>
    /// Pause all the sounds included in audioManager
    /// </summary>
    public void PauseAllSounds()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.source.isPlaying)
            {
                sound.source.Pause();
                currentSounds.Add(sound);
            }
        }
    }

    /// <summary>
    /// Play all the sounds that have been paused
    /// </summary>
    public void ResumeAllSounds()
    {
        foreach (Sound sound in currentSounds)
        {
            sound.source.UnPause();
        }
    }

    /// <summary>
    /// Play all the sounds that have been paused
    /// </summary>
    public void MuteAllSounds()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.mute = true;
        }
    }

    /// <summary>
    /// Play all the sounds that have been paused
    /// </summary>
    public void UnMuteAllSounds()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.mute = false;
        }
    }

    /// <summary>
    /// Stop all the sounds included in audiomanager
    /// </summary>
    public void StopAllSounds()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
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
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * volume;
        }
    }


    /// <summary>
    /// Know if a sound is playing
    /// </summary>
    /// <param name="name">Sound name</param>
    /// <returns></returns>
    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not exist");
            return false;
        }

        return s.source.isPlaying;
    }
}
