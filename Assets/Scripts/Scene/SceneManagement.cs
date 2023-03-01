using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;
    private static Animator animator;
    private static GameObject loadingScreen;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("AUDIO"));
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("VISUAL"));
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("CONTROLS"));
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("UI"));
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        animator = GameObject.FindGameObjectWithTag("TRANSITION").GetComponent<Animator>();

        StartCoroutine(EndLoading());

        AudioSaver.ApplyChanges();
        VisualSaver.ApplyChanges();
    }

    private IEnumerator EndLoading()
    {
        animator.SetBool("endLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private static IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        TMP_Text percentage = loadingScreen.GetComponentInChildren<TMP_Text>();

        while (!operation.isDone)
        {
            percentage.text = (int) (operation.progress * 100) + " %";

            yield return null;
        }
    }

    /// <summary>
    /// Load the next scene in the list of build scenes
    /// </summary>
    public static IEnumerator LoadNextScene() 
    {
        animator.SetBool("isLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Load the previous scene in the list of build scenes
    /// </summary>
    public static IEnumerator LoadPreviousScene()
    {
        animator.SetBool("isLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    /// <summary>
    /// Load a scene by the scene name
    /// </summary>
    /// <param name="sceneName"> The scene name </param>
    public static void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Load a scene by the scene name
    /// </summary>
    /// <param name="sceneName"> The scene name </param>
    public static IEnumerator LoadSceneByIndex(int sceneIndex)
    {
        animator.SetBool("isLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);

        loadingScreen = animator.transform.GetChild(0).gameObject;
        loadingScreen.SetActive(true);
        Instance.StartCoroutine(LoadSceneAsync(sceneIndex));
    }
}
