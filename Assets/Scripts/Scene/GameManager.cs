using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private TransitionPlayer transitionPlayer;
    [SerializeField] private SaveOptions defaultOptions;
    [SerializeField] private SaveGame defaultGame;

    [Header("SceneSettings")]
    [SerializeField] private List<SceneLogic> scenes;
    [SerializeField] private Object firstSceneObject;

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

    public static ref readonly PlayerInput PlayerInput { get => ref playerInput; }
    public static ref readonly InputSystemUIInputModule UiInput { get => ref uiInput; }
    public static ref readonly ControllerRumble ControllerRumble { get => ref controllerRumble; }
    public static ref readonly SceneController SceneController { get => ref sceneController; }
    public static ref readonly InputMapping InputMapping { get => ref inputMapping; }
    public static ref readonly InputDetection InputDetection { get => ref inputDetection; }

    public static int RANDOM_SEED => System.Guid.NewGuid().GetHashCode();

    private void Awake()
    {
        inputReader.Initialize();
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();
        uiInput = GetComponent<InputSystemUIInputModule>();

        if(firstSceneObject != null)
            if (SceneManager.GetActiveScene().name == firstSceneObject.name)
            {
                DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MUSIC").transform.parent.gameObject);
                DontDestroyOnLoad(gameObject);
            }

        SceneManager.sceneLoaded += OnSceneLoaded;

        saver = new DataSaver(defaultOptions, defaultGame);
        applier = new OptionsApplier(audioMixer);

        saver.Load();

        inputMapping = new();
        inputDetection = new();
        sceneController = new(SceneManager.GetActiveScene().name, scenes);
        transitionPlayer.Initialize();
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
        if(Time.timeScale < 1)
            Time.timeScale = 1;

        if (firstSceneObject != null)
            if (SceneManager.GetActiveScene().name != firstSceneObject.name)
                playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();

        InputDetection.Configure();
    }

    private void Update()
    {
        inputDetection.CheckCustomControlScheme();
    }
}
