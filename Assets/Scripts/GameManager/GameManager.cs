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

    private static SceneController sceneController;
    private static AudioController audioController;
    private static InputUtilities inputUtilities;

    public static ref readonly SceneController SceneController { get => ref sceneController; }
    public static ref readonly InputUtilities InputUtilities { get => ref inputUtilities; }
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
        inputUtilities = new(GetComponent<InputSystemUIInputModule>(), inputReader);
    }

    private void Start()
    {
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

        inputUtilities.Configure();
    }

    private void Update()
    {
        inputUtilities.Update();
    }
}
