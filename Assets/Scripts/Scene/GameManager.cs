using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SaveConfiguration saveConfig;

    [Header("Sounds")]
    [SerializeField] private Tuple<Sounds, SceneNumber>[] allSounds;

    private string lastControlScheme;

    private ISave saver;
    private IApplier applier;

    private static PlayerInput playerInput;
    private static InputSystemUIInputModule uiInput;
    private static ControllerRumble controllerRumble;
    private static SceneLoader sceneLoader;
    private static InputMapping inputMapping;
    private static InputDetection inputDetection;

    public static ref readonly PlayerInput PlayerInput { get => ref playerInput; }
    public static ref readonly InputSystemUIInputModule UiInput { get => ref uiInput; }
    public static ref readonly ControllerRumble ControllerRumble { get => ref controllerRumble; }
    public static ref readonly SceneLoader SceneLoader { get => ref sceneLoader; }
    public static ref readonly InputMapping InputMapping { get => ref inputMapping; }
    public static ref readonly InputDetection InputDetection { get => ref inputDetection; }

    private void Awake()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();
        uiInput = GetComponent<InputSystemUIInputModule>();

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneNumber.MAIN_MENU)
        {
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MUSIC").transform.parent.gameObject);
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        saver = new DataSaver(saveConfig);
        applier = new OptionsApplier(audioMixer);

        saver.Load();

        inputMapping = new();
        inputDetection = new();
    }

    private void Start()
    {
        controllerRumble = new();
        sceneLoader = new();

        applier.ApplyChanges();
    }

    private void OnDestroy()
    {
        saver.Save();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)SceneNumber.MAIN_MENU)
            playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();
        

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
