using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0f, 3f)]
    public float pitch;

    public bool loop;

    [System.NonSerialized]
    public AudioSource[] source;
}

[System.Serializable]
public struct SoundGroup
{
    public string[] speakersTag;
    public Sound[] sounds;
    [System.NonSerialized] public GameObject[] speakers;
}
