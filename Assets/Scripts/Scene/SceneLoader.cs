using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private AsyncOperation asyncOperation;

    public static event System.Func<Task> StartTransition;
    public static event System.Func<Task> EndTransition;

    public static event System.Action ActivateLoading;
    public static event System.Func<float,bool> UpdateLoading;

    private static int nextSceneIndex;

    public SceneLoader()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EndLoad();

        if (SceneManager.GetActiveScene().buildIndex == (int) SceneNumber.LOADING)
        {
            ActivateLoading.Invoke();
            LoadSceneAsync();
        }
    }

    private async void EndLoad()
    {
        await EndTransition.Invoke();
    }

    public async void LoadScene(SceneNumber sceneIndex)
    {
        await StartTransition.Invoke();
        GameManager.PlayerInput.controlsChangedEvent.RemoveAllListeners();
        SceneManager.LoadScene((int) sceneIndex);
    }

    public async void LoadWithLoadingScreen(SceneNumber nextScene)
    {
        await StartTransition.Invoke();
        nextSceneIndex = (int) nextScene;
        GameManager.PlayerInput.controlsChangedEvent.RemoveAllListeners();
        SceneManager.LoadScene((int) SceneNumber.LOADING);
    }

    private async void LoadSceneAsync()
    {
        await Task.Delay(1000);
        asyncOperation = SceneManager.LoadSceneAsync(nextSceneIndex);
        asyncOperation.allowSceneActivation = false;

        while(UpdateLoading.Invoke(asyncOperation.progress) == false)
            await Task.Delay(100);

        AllowScene();
    }

    private async void AllowScene()
    {
        await StartTransition.Invoke();
        asyncOperation.allowSceneActivation = true;
    }
}
