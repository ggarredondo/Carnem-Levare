using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInitializer : MonoBehaviour
{
    [SerializeField] private string ID;
    [SerializeField] private List<SoundStructure> soundStructures;
    [SerializeField] private bool destroySoundStructures;

    private List<Hashtable> soundTables;

    public void Initialize()
    {
        soundTables = new List<Hashtable>(soundStructures.Count);

        for (int i = 0; i < soundStructures.Count; i++)
        {
            Hashtable hastable = new Hashtable();

            foreach (SoundGroup group in soundStructures[i].SoundGroups)
            {
                foreach (Sound s in group.sounds)
                {
                    s.source = gameObject.AddComponent<AudioSource>();
                    s.source.clip = s.clip;
                    s.source.volume = s.volume;
                    s.source.pitch = s.pitch;
                    s.source.loop = s.loop;
                    s.source.spatialBlend = soundStructures[i].threeD ? 1 : 0;

                    if (soundStructures[i].threeD)
                    {
                        s.source.dopplerLevel = soundStructures[i].dopplerLevel;
                        s.source.spread = soundStructures[i].spread;
                        s.source.rolloffMode = soundStructures[i].volumeRolloff;
                        s.source.minDistance = soundStructures[i].minDistance;
                        s.source.maxDistance = soundStructures[i].maxDistance;
                        s.source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, soundStructures[i].customRollofCurve);
                    }

                    s.source.outputAudioMixerGroup = soundStructures[i].audioMixerGroup;

                    if (ID != "")
                        hastable.Add(ID + "_" + s.name, s.source);
                    else
                        hastable.Add(s.name, s.source);
                }
            }

            soundTables.Add(hastable);
        }

        GameManager.Audio.InitializeSoundSources(ref soundStructures, ref soundTables);
    }


    private void Awake()
    {
        AudioController.InitializeSound += Initialize;
    }

    private void OnDestroy()
    {
        AudioController.InitializeSound -= Initialize;

        if(destroySoundStructures)
            GameManager.Audio.DeleteSoundSources(ref soundStructures, ref soundTables);
    }
}
