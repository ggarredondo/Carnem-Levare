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
    private TMP_Text percentage;

    private string lastControlScheme;
    private int controlSchemeIndex;
    private string continueAction;

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
    public int ControlSchemeIndex { get { return controlSchemeIndex; } }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        animator = GameObject.FindGameObjectWithTag("TRANSITION").GetComponent<Animator>();
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        lastControlScheme = playerInput.defaultControlScheme;
        controlSchemeIndex = 0;

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

    private void Update()
    {
        if (playerInput.currentControlScheme != lastControlScheme)
        {
            lastControlScheme = playerInput.currentControlScheme;
            controlSchemeIndex = (controlSchemeIndex + 1) % 2;
            continueAction = playerInput.actions.FindAction("Continue").bindings[controlSchemeIndex].path.Split("/")[1];
        }
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneId);
        asyncOperation.allowSceneActivation = false;

        GameObject loadingScreen = animator.transform.GetChild(0).gameObject;
        percentage = loadingScreen.GetComponentInChildren<TMP_Text>();

        loadingScreen.SetActive(true);

        playerInput.SwitchCurrentActionMap("LoadingScreen");

        while (!asyncOperation.isDone)
        {
            percentage.text = (int) (Mathf.Clamp01(asyncOperation.progress / 0.9f) * 100) + " %";

            if (asyncOperation.progress >= 0.9f)
            {
                percentage.text = "Press " + continueAction + " to continue";

                if (playerInput.actions.FindAction("Continue").IsPressed())
                    asyncOperation.allowSceneActivation = true;
            }

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
