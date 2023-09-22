using UnityEngine;
using UnityEngine.Audio;

public class OptionsApplier : IApplier
{
    private readonly AudioMixer audioMixer;

    public OptionsApplier(in AudioMixer audioMixer)
    {
        this.audioMixer = audioMixer;
    }

    public void ApplyChanges(OptionsSlot options)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(options.masterVolume) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(options.musicVolume) * 20);
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(options.sfxVolume) * 20);

        if (options.mute) audioMixer.SetFloat("MasterVolume", -80);
        else audioMixer.SetFloat("MasterVolume", Mathf.Log10(options.masterVolume) * 20);

        QualitySettings.vSyncCount = options.vSync ? 1 : 0;

        string[] resolutionArray = options.resolution.Split('x');
        Screen.SetResolution(int.Parse(resolutionArray[0]), int.Parse(resolutionArray[1]), options.fullscreen);

        QualitySettings.SetQualityLevel(options.quality, false);
    }
}
