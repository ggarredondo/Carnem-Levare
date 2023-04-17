using UnityEngine;
using UnityEngine.Audio;

public class AudioApplier : IOptionsApplier
{ 
    private AudioMixer audioMixer;

    public AudioApplier(AudioMixer audioMixer)
    {
        this.audioMixer = audioMixer;
    }

    public void ApplyChanges()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(DataSaver.options.masterVolume) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(DataSaver.options.musicVolume) * 20);
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(DataSaver.options.sfxVolume) * 20);

        if (DataSaver.options.mute) audioMixer.SetFloat("MasterVolume", -80);
        else audioMixer.SetFloat("MasterVolume", Mathf.Log10(DataSaver.options.masterVolume) * 20);
    }
}
