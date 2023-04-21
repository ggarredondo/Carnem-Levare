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

    private static PlayerInput playerInput;
    private static InputSystemUIInputModule uiInput;
    private static ControllerRumble controllerRumble;
    private static SceneLoader sceneLoader;

    public static PlayerInput PlayerInput { get { return playerInput; } }

    public static InputSystemUIInputModule UiInput { get { return uiInput; } }

    public static ControllerRumble ControllerRumble { get { return controllerRumble; } }

    public static SceneLoader SceneLoader { get { return sceneLoader; } }

    private void Awake()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneNumber.MAIN_MENU)
        {
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MUSIC").transform.parent.gameObject);
            DontDestroyOnLoad(gameObject);
        }

        uiInput = GetComponent<InputSystemUIInputModule>();
        SceneManager.sceneLoaded += OnSceneLoaded;

        saver = new DataSaver(saveConfig);
        applier = new OptionsApplier(audioMixer);

        saver.Load();
    }

    private void Start()
    {
        controllerRumble = gameObject.AddComponent<ControllerRumble>();
        sceneLoader = gameObject.AddComponent<SceneLoader>();

        applier.ApplyChanges();
    }

    private void OnDestroy()
    {
        saver.Save();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();

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
