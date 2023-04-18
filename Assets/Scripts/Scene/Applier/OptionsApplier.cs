using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class OptionsApplier : IApplier
{
    private readonly AudioMixer audioMixer;
    public static UnityAction apply;

    public OptionsApplier(AudioMixer audioMixer)
    {
        this.audioMixer = audioMixer;
        apply += ApplyChanges;
    }

    public void ApplyChanges()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(DataSaver.options.masterVolume) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(DataSaver.options.musicVolume) * 20);
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(DataSaver.options.sfxVolume) * 20);

        if (DataSaver.options.mute) audioMixer.SetFloat("MasterVolume", -80);
        else audioMixer.SetFloat("MasterVolume", Mathf.Log10(DataSaver.options.masterVolume) * 20);

        QualitySettings.vSyncCount = DataSaver.options.vSync;

        string[] resolutionArray = DataSaver.options.resolution.Split('x');
        Screen.SetResolution(int.Parse(resolutionArray[0]), int.Parse(resolutionArray[1]), DataSaver.options.fullscreen);

        QualitySettings.SetQualityLevel(DataSaver.options.quality, false);
    }
}
