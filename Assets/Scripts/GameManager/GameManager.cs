using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] private VisualData visualOptionsData;

    [Header("SceneSettings")]
    [SerializeField] private string firstSceneName;
    [SerializeField] private List<SceneLogic> scenes;
    [SerializeField] private TransitionPlayer transitionPlayer;

    [Header("Debug")]
    [SerializeField] [Range(0f, 1f)] private float timeScale = 1f;

    private static ISave saver;
    private static IChangeScene sceneController;
    private static InputUtilities inputUtilities;
    private static AudioController audioController;

    public static ref readonly ISave Save { get => ref saver; }
    public static ref readonly IChangeScene Scene { get => ref sceneController; }
    public static ref readonly InputUtilities Input { get => ref inputUtilities; }
    public static ref readonly AudioController Audio { get => ref audioController; }

    public static int RANDOM_SEED => System.Guid.NewGuid().GetHashCode();

    private void Awake()
    {
        if(firstSceneName != "") DontDestroyOnLoad(gameObject);

        //Audio Initialize
        audioController = new();

        //Scene Initialize
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        sceneController = new SceneController(SceneManager.GetActiveScene().name, scenes);
        transitionPlayer.Initialize();

        //Save Initialize
        visualOptionsData.Initialize();
        saver = new DataSaver(defaultOptions, defaultGame, audioMixer);
        saver.Load();

        //Input Initialize
        inputUtilities = new(GetComponent<InputSystemUIInputModule>(), inputReader);
    }

    private void Start()
    {
        saver.ApplyChanges();
    }

    private void OnDestroy()
    {
        saver.Save();
    }

    private void Update()
    {
        inputUtilities.Update();
    }

    private void OnValidate()
    {
        Time.timeScale = timeScale;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;

        inputUtilities.Configure();

        if (scene.name != sceneController.GetCurrentLoadScene())
        {
            SceneLogic currentSceneLogic = sceneController.GetSceneLogic(scene.name);

            if (currentSceneLogic.gameObjectsTag.Count > 0)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");

                foreach (string tag in currentSceneLogic.gameObjectsTag)
                    GameObject.FindGameObjectWithTag(tag).GetComponent<IObjectInitialize>().Initialize(ref player, ref enemy);
            }

            AudioController.InitializeSound?.Invoke();

            if (currentSceneLogic.playMusic)
                audioController.PlayMusic(currentSceneLogic.music);
        }
    }

    void OnSceneUnloaded(Scene scene)
    {
        AudioController.InitializeSound = null;
    }
}
