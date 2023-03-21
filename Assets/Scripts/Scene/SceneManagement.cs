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
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("SAVE"));
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("UI"));
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MUSIC").transform.parent.gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool TransitionEnd { get { return transitionEnd; } }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        animator = GameObject.FindGameObjectWithTag("TRANSITION").GetComponent<Animator>();
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();
        AudioManager gameSfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            loadingScreen = GameObject.FindGameObjectWithTag("LOADING").GetComponent<LoadingScreen>();
            StartCoroutine(LoadFromLoadingScreen());
        }

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            gameSfxManager.speakers = GameObject.FindGameObjectsWithTag("AUDIO_SOURCES");
            gameSfxManager.Initialize();
        }
        else gameSfxManager.NotActive = true;

        playerInput.controlsChangedEvent.AddListener(ControlSaver.OnControlSchemeChanged);
        ControlSaver.OnControlSchemeChanged(playerInput);

        transitionEnd = false;
        StartCoroutine(EndLoading());
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
