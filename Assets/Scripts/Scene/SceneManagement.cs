using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;
    public AnimationClip endAnimation;

    private Animator animator;
    private bool transitionEnd;
    private PlayerInput playerInput;
    private AsyncOperation asyncOperation;
    private LoadingScreen loadingScreen;

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
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("AUDIO"));
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("VISUAL"));
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("CONTROLS"));
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("UI"));
        DontDestroyOnLoad(gameObject);
    }

    public bool TransitionEnd { get { return transitionEnd; } }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        animator = GameObject.FindGameObjectWithTag("TRANSITION").GetComponent<Animator>();
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();
        loadingScreen = animator.GetComponent<LoadingScreen>();

        playerInput.controlsChangedEvent.AddListener(ControlSaver.OnControlSchemeChanged);
        ControlSaver.OnControlSchemeChanged(playerInput);

        transitionEnd = false;
        StartCoroutine(EndLoading());

        AudioSaver.ApplyChanges();
        VisualSaver.ApplyChanges();
    }

    private IEnumerator EndLoading()
    {
        animator.SetBool("endLoading", true);
        yield return new WaitForSecondsRealtime(endAnimation.length);
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
                asyncOperation.allowSceneActivation = true;
            
            yield return null;
        }
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

    /// <summary>
    /// Load a scene by the scene name
    /// </summary>
    /// <param name="sceneName"> The scene name </param>
    public IEnumerator LoadSceneByIndex(int sceneIndex)
    {
        yield return new WaitUntil(() => TransitionEnd);
        animator.SetBool("isLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
}
