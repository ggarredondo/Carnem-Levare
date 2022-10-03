using UnityEngine;

public class AudioMixer : MonoBehaviour
{
    [Header("Audio Mixer")]
    [Range(0f, 100f)] public float globalVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Audio Managers")]
    public AudioManager sfxManager;
    public AudioManager musicManager;

    private void Update()
    {
        AudioListener.volume = globalVolume;
        sfxManager.ChangeVolume(sfxVolume);
        musicManager.ChangeVolume(musicVolume);
    }
}
