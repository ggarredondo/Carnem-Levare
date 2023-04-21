using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    private AsyncOperation asyncOperation;

    public delegate IEnumerator Transition();
    public static Transition startTransition;
    public static Transition endTransition;

    public static UnityAction activateLoading;
    public delegate bool UpdateLoading(float progress);
    public static UpdateLoading updateLoading;

    private static int nextSceneIndex;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(EndLoad());

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneNumber.LOADING_MENU)
        {
            activateLoading.Invoke();
            LoadSceneAsync();
        }
    }

    private IEnumerator EndLoad()
    {
        yield return endTransition.Invoke();
    }

    public IEnumerator LoadScene(int sceneIndex)
    {
        yield return startTransition.Invoke();
        SceneManager.LoadScene(sceneIndex);
    }

    public IEnumerator LoadWithLoadingScreen(int nextScene)
    {
        yield return startTransition.Invoke();
        nextSceneIndex = nextScene;
        SceneManager.LoadScene((int) SceneNumber.LOADING_MENU);
    }

    private async void LoadSceneAsync()
    {
        await Task.Delay(1000);
        asyncOperation = SceneManager.LoadSceneAsync(nextSceneIndex);
        asyncOperation.allowSceneActivation = false;

        while(updateLoading.Invoke(asyncOperation.progress) == false)
            await Task.Delay(100);

        StartCoroutine(AllowScene());
    }

    private IEnumerator AllowScene()
    {
        yield return startTransition.Invoke();
        asyncOperation.allowSceneActivation = true;
    }
}

public enum SceneNumber
{
    MAIN_MENU = 0,
    LOADING_MENU = 1,
    NON_DESTROY_MAIN_MENU = 2,
    GAME = 3
}
