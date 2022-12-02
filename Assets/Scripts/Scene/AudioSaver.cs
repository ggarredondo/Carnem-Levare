using UnityEngine;

public class AudioSaver : MonoBehaviour
{
    public const float GLOBAL_MAX = 5f;

    [Header("Audio Mixer")]
    [Range(0f, GLOBAL_MAX)] public static float globalVolume = 1f;
    [Range(0f, 1f)] public static float musicVolume = 1f;
    [Range(0f, 1f)] public static float sfxVolume = 1f;
    public static bool mute = false;

    private static AudioManager sfxManager;
    private static AudioManager musicManager;

    public static void ApplyChanges()
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

        //PERMANENT CHANGES
        SaveManager.Instance.activeSave.globalVolume = globalVolume;
        SaveManager.Instance.activeSave.musicVolume = musicVolume;
        SaveManager.Instance.activeSave.sfxVolume = sfxVolume;
        SaveManager.Instance.activeSave.mute = mute;
    }

    public static void LoadChanges()
    {
        globalVolume = SaveManager.Instance.activeSave.globalVolume;
        musicVolume = SaveManager.Instance.activeSave.musicVolume;
        sfxVolume = SaveManager.Instance.activeSave.sfxVolume;
        mute = SaveManager.Instance.activeSave.mute;
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

    public static void MuteAll()
    {
        sfxManager.MuteAllSounds();
        musicManager.MuteAllSounds();
    }

    public static void UnMuteAll()
    {
        sfxManager.UnMuteAllSounds();
        musicManager.UnMuteAllSounds();
    }

}
