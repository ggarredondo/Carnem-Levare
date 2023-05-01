
[System.Serializable]
public class OptionsSlot : SaveSlot
{
    public bool mute;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    public bool vSync;
    public bool fullscreen;
    public string resolution;
    public int quality;

    public bool rumble;
    public string rebinds;
}
