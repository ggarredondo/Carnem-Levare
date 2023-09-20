using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("InputSettings")]
    [SerializeField] private InputReader inputReader;

    [Header("SaveSettings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SaveOptions defaultOptions;
    [SerializeField] private SaveGame defaultGame;

    [Header("SceneSettings")]
    [SerializeField] private string firstSceneName;
    [SerializeField] private List<SceneLogic> scenes;
    [SerializeField] private TransitionPlayer transitionPlayer;

    [Header("Debug")]
    [SerializeField] [Range(0f, 1f)] private float timeScale = 1f;

    private ISave saver;
    private IApplier applier;

    private static PlayerInput playerInput;
    private static InputSystemUIInputModule uiInput;
    private static ControllerRumble controllerRumble;
    private static SceneController sceneController;
    private static InputMapping inputMapping;
    private static InputDetection inputDetection;
    private static AudioController audioController;

    public static ref readonly PlayerInput PlayerInput { get => ref playerInput; }
    public static ref readonly InputSystemUIInputModule UiInput { get => ref uiInput; }
    public static ref readonly ControllerRumble ControllerRumble { get => ref controllerRumble; }
    public static ref readonly SceneController SceneController { get => ref sceneController; }
    public static ref readonly InputMapping InputMapping { get => ref inputMapping; }
    public static ref readonly InputDetection InputDetection { get => ref inputDetection; }
    public static ref readonly AudioController AudioController { get => ref audioController; }

    public static int RANDOM_SEED => System.Guid.NewGuid().GetHashCode();

    private void Awake()
    {
        //Audio Initialize
        audioController = new();

        //Scene Initialize
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        sceneController = new(SceneManager.GetActiveScene().name, scenes);
        transitionPlayer.Initialize();

        //Save Initialize
        saver = new DataSaver(defaultOptions, defaultGame);
        applier = new OptionsApplier(audioMixer);
        saver.Load();

        //Input Initialize
        inputReader.Initialize();
        uiInput = GetComponent<InputSystemUIInputModule>();
        playerInput = PlayerInput.all[0];
        inputMapping = new();
        inputDetection = new();
        controllerRumble = new();
    }

    private void Start()
    {
        controllerRumble = new();
        applier.ApplyChanges();
    }

    private void OnValidate()
    {
        Time.timeScale = timeScale;
    }

    private void OnDestroy()
    {
        saver.Save();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;

        playerInput = PlayerInput.all[0];

        inputDetection.Configure();
    }

    private void Update()
    {
        inputDetection.CheckCustomControlScheme();
    }
}
