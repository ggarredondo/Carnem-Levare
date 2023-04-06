using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;

    private Animator animator;
    private bool transitionEnd;
    private PlayerInput playerInput;
    private AsyncOperation asyncOperation;
    private LoadingScreen loadingScreen;
    private int sceneToLoad;
    [SerializeField] private Tuple<Sounds, SceneNumber>[] allSounds;

    private void Awake()
    {
        Instance = this;
        Application.backgroundLoadingPriority = ThreadPriority.Low;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int) SceneNumber.MAIN_MENU)
        {
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("SAVE"));
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("UI"));
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MUSIC").transform.parent.gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    #region Public

    public bool TransitionEnd { get { return transitionEnd; } }

    public PlayerInput PlayerInput { get { return playerInput; } }

    /// <summary>
    /// Event that trigger when an scene is loaded
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Obtain the current transition animator and the current playerInput
        animator = GameObject.FindGameObjectWithTag("TRANSITION").GetComponent<Animator>();
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();

        //If the actual scene is a loading scene, starts to loading the next scene
        if (SceneManager.GetActiveScene().buildIndex == (int) SceneNumber.LOADING_MENU)
        {
            loadingScreen = GameObject.FindGameObjectWithTag("LOADING").GetComponent<LoadingScreen>();
            StartCoroutine(LoadFromLoadingScreen());
        }

        //Initialize the sounds sources of the scene
        InitializeSoundSources();

        //Asign the controlsChangedEvent to the actual playerInput
        playerInput.controlsChangedEvent.AddListener(ControlSaver.Instance.OnControlSchemeChanged);
        ControlSaver.Instance.OnControlSchemeChanged(playerInput);

        //Show the opening scene transition
        StartCoroutine(EndLoading());
    }

    /// <summary>
    /// Load the previous scene in the list of build scenes
    /// </summary>
    public IEnumerator LoadPreviousScene()
    {
        yield return new WaitUntil(() => TransitionEnd);
        animator.SetBool("isLoading", true);
        transitionEnd = false;
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    /// <summary>
    /// Load a scene on the default loading screen
    /// </summary>
    /// <param name="sceneIndex">The scene number</param>
    public IEnumerator LoadSceneByIndexAsync(int sceneIndex)
    {
        //Finish the opening scene animation
        yield return new WaitUntil(() => TransitionEnd);

        //Starting the endScene animation
        animator.SetBool("isLoading", true);
        transitionEnd = false;
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);

        //Store the scene to load in the loading screen
        sceneToLoad = sceneIndex;

        //Load the loading screen
        SceneManager.LoadScene(1);
    }

    #endregion

    #region Private

    /// <summary>
    /// Initialize the AudioSources of the scene
    /// </summary>
    private void InitializeSoundSources()
    {
        for(int i = 0; i < allSounds.GetLength(0); i++)
        {
            if (SceneManager.GetActiveScene().buildIndex == (int) allSounds[i].Item2)
            {
                for (int j = 0; j < allSounds[i].Item1.SoundGroups.GetLength(0); j++)
                {
                    GameObject[] speakers = GameObject.FindGameObjectsWithTag(allSounds[i].Item1.SoundGroups[j].speakersTag);
                    allSounds[i].Item1.SoundGroups[j].speakers = speakers;
                }

                allSounds[i].Item1.Initialize();
            }
        }
    }

    /// <summary>
    /// Starts the ending load animation
    /// </summary>
    private IEnumerator EndLoading()
    {
        animator.SetBool("endLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        transitionEnd = true;
    }

    /// <summary>
    /// Load a scene asynchronous
    /// </summary>
    /// <param name="sceneId"></param>
    /// <returns></returns>
    private async void LoadSceneAsync(int sceneId)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneId);

        //Not load the screen just when async loading finished
        asyncOperation.allowSceneActivation = false;

        //Activate the loading screen UI
        loadingScreen.Activate();
        bool done;

        do
        {
            await Task.Delay(100);
            done = loadingScreen.UpdateProgess(asyncOperation.progress);
        } while (!done);

        StartCoroutine(EndAsyncOperation());
    }

    /// <summary>
    /// Finish the load async operation
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndAsyncOperation()
    {
        animator.SetBool("isLoading", true);
        transitionEnd = false;
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        asyncOperation.allowSceneActivation = true;
    }

    /// <summary>
    /// Wait the opening scene animation and starts load asynchronous
    /// </summary>
    private IEnumerator LoadFromLoadingScreen()
    {
        yield return new WaitUntil(() => TransitionEnd);
        LoadSceneAsync(sceneToLoad);
    }

    #endregion
}

/// <summary>
/// Enumerator that defines the relevant scene numbers
/// </summary>
public enum SceneNumber
{
    MAIN_MENU = 0,
    LOADING_MENU = 1,
    GAME = 3
}
