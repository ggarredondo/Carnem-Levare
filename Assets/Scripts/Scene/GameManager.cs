using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SaveConfiguration saveConfig;

    private ISave saver;
    private IApplier applier;

    private void Awake()
    {
        saver = new DataSaver(saveConfig);
        applier = new OptionsApplier(audioMixer);

        saver.Load();
    }

    private void Start()
    {
        applier.ApplyChanges();
    }

    private void OnDestroy()
    {
        saver.Save();
    }
}
