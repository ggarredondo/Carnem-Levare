using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SaveConfiguration saveConfig;

    [Header("Sounds")]
    [SerializeField] private Tuple<Sounds, SceneNumber>[] allSounds;

    private ISave saver;
    private IApplier applier;

    public static PlayerInput PlayerInput { get; private set; }
    public static InputSystemUIInputModule UiInput { get; private set; }
    public static ControllerRumble ControllerRumble { get; private set; }
    public static SceneLoader SceneLoader { get; private set; }
    public static InputMapping InputMapping { get; private set; }
    public static InputDetection InputDetection { get; private set; }

    private void Awake()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        PlayerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();
        UiInput = GetComponent<InputSystemUIInputModule>();

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneNumber.MAIN_MENU)
        {
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MUSIC").transform.parent.gameObject);
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        saver = new DataSaver(saveConfig);
        applier = new OptionsApplier(audioMixer);

        saver.Load();

        InputMapping = new();
        InputDetection = new();
    }

    private void Start()
    {
        ControllerRumble = new();
        SceneLoader = gameObject.AddComponent<SceneLoader>();

        applier.ApplyChanges();
    }

    private void OnDestroy()
    {
        saver.Save();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)SceneNumber.MAIN_MENU)
            PlayerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();

        InputDetection.Configure();

        InitializeSoundSources();
    }

    private void InitializeSoundSources()
    {
        for (int i = 0; i < allSounds.GetLength(0); i++)
        {
            if (SceneManager.GetActiveScene().buildIndex == (int)allSounds[i].Item2)
            {
                for (int j = 0; j < allSounds[i].Item1.SoundGroups.GetLength(0); j++)
                {
                    allSounds[i].Item1.SoundGroups[j].speakers = new GameObject[allSounds[i].Item1.SoundGroups[j].speakersTag.GetLength(0)];

                    for (int k = 0; k < allSounds[i].Item1.SoundGroups[j].speakersTag.GetLength(0); k++)
                    {
                        GameObject speaker = GameObject.FindGameObjectWithTag(allSounds[i].Item1.SoundGroups[j].speakersTag[k]);
                        allSounds[i].Item1.SoundGroups[j].speakers[k] = speaker;
                    }
                }

                allSounds[i].Item1.Initialize();
            }
        }
    }
}
