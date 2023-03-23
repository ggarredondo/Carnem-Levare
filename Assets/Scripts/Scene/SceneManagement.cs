using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    public bool TransitionEnd { get { return transitionEnd; } }

    public PlayerInput PlayerInput { get { return playerInput; } }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        animator = GameObject.FindGameObjectWithTag("TRANSITION").GetComponent<Animator>();
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();

        if (SceneManager.GetActiveScene().buildIndex == (int) SceneNumber.LOADING_MENU)
        {
            loadingScreen = GameObject.FindGameObjectWithTag("LOADING").GetComponent<LoadingScreen>();
            StartCoroutine(LoadFromLoadingScreen());
        }

        InitializeSoundSources();

        playerInput.controlsChangedEvent.AddListener(ControlSaver.OnControlSchemeChanged);
        ControlSaver.OnControlSchemeChanged(playerInput);

        transitionEnd = false;
        StartCoroutine(EndLoading());
    }

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

    private IEnumerator EndLoading()
    {
        animator.SetBool("endLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        transitionEnd = true;
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneId);
        asyncOperation.allowSceneActivation = false;

        loadingScreen.Activate();

        while (!asyncOperation.isDone)
        {
            if (loadingScreen.UpdateProgess(asyncOperation.progress))
            {
                StartCoroutine(EndAsyncOperation());
                break;
            }

            yield return null;
        }
    }

    public IEnumerator EndAsyncOperation()
    {
        animator.SetBool("isLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        asyncOperation.allowSceneActivation = true;
    }

    /// <summary>
    /// Load the previous scene in the list of build scenes
    /// </summary>
    public IEnumerator LoadPreviousScene()
    {
        yield return new WaitUntil(() => TransitionEnd);
        animator.SetBool("isLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public IEnumerator LoadFromLoadingScreen()
    {
        yield return new WaitUntil(() => TransitionEnd);
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    /// <summary>
    /// Load a scene by the scene name
    /// </summary>
    /// <param name="sceneName"> The scene name </param>
    public IEnumerator LoadSceneByIndexAsync(int sceneIndex)
    {
        yield return new WaitUntil(() => TransitionEnd);
        animator.SetBool("isLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        sceneToLoad = sceneIndex;
        SceneManager.LoadScene(1);
    }
}

public enum SceneNumber
{
    MAIN_MENU = 0,
    LOADING_MENU = 1,
    GAME = 3
}
