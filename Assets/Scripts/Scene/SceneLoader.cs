using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private AsyncOperation asyncOperation;

    public static System.Func<Task> startTransition;
    public static System.Func<Task> endTransition;

    public static System.Action activateLoading;
    public static System.Func<float,bool> updateLoading;

    private static int nextSceneIndex;

    public SceneLoader()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EndLoad();

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneNumber.LOADING_MENU)
        {
            activateLoading.Invoke();
            LoadSceneAsync();
        }
    }

    private async void EndLoad()
    {
        await endTransition.Invoke();
    }

    public async void LoadScene(int sceneIndex)
    {
        await startTransition.Invoke();
        SceneManager.LoadScene(sceneIndex);
    }

    public async void LoadWithLoadingScreen(int nextScene)
    {
        await startTransition.Invoke();
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

        AllowScene();
    }

    private async void AllowScene()
    {
        await startTransition.Invoke();
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
