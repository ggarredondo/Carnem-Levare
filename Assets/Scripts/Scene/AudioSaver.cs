using UnityEngine;

public class AudioSaver : MonoBehaviour
{
    public const float GLOBAL_MAX = 5f;

    [Header("Audio Mixer")]
    [Range(0f, GLOBAL_MAX)] public float globalVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    public bool mute = false;

    private AudioManager sfxManager;
    private AudioManager musicManager;

    public void ApplyChanges()
    {
        sfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();
        musicManager = GameObject.FindGameObjectWithTag("MUSIC").GetComponent<AudioManager>();

        AudioListener.volume = globalVolume;
        sfxManager.ChangeVolume(sfxVolume);
        musicManager.ChangeVolume(musicVolume);

        if (mute)
            MuteAll();
        else
            UnMuteAll();
    }

    public void PauseAll()
    {
        sfxManager.PauseAllSounds();
        musicManager.PauseAllSounds();
    }

    public void ResumeAll()
    {
        sfxManager.ResumeAllSounds();
        musicManager.ResumeAllSounds();
    }

    public void MuteAll()
    {
        sfxManager.MuteAllSounds();
        musicManager.MuteAllSounds();
    }

    public void UnMuteAll()
    {
        sfxManager.UnMuteAllSounds();
        musicManager.UnMuteAllSounds();
    }

}
