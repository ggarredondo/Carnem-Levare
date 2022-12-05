using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    private void Awake()
    {
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
        AudioSaver.ApplyChanges();
        VisualSaver.ApplyChanges();
    }
    
    /// <summary>
    /// Load the next scene in the list of build scenes
    /// </summary>
    public static void LoadNextScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Load the previous scene in the list of build scenes
    /// </summary>
    public static void LoadPreviousScene()
    {
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
    public static void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

}
