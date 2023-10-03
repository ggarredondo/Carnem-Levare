using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : IChangeScene
{
    private Dictionary<string, SceneLogic> scenesTable;
    private List<SceneLogic> scenes;
    private readonly SceneLoader sceneLoader;

    private string currentScene;
    private string currentLoadScene;

    public SceneController(string initialScene, List<SceneLogic> scenes)
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        sceneLoader = new();
        scenesTable = new();

        currentScene = initialScene;

        this.scenes = scenes;
        Initialize();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != currentLoadScene)
        {
            if (scenesTable[scene.name].gameObjectsTag.Count > 0)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");

                foreach (string tag in scenesTable[scene.name].gameObjectsTag)
                    GameObject.FindGameObjectWithTag(tag).GetComponent<IObjectInitialize>().Initialize(ref player, ref enemy);
            }

            AudioController.InitializeSound?.Invoke();
            PlayMusic();
        }
    }

    void OnSceneUnloaded(Scene scene)
    {
        AudioController.InitializeSound = null;
    }

    private void Initialize()
    {
        scenes.ForEach(s => scenesTable.Add(s.sceneName, s));
    }

    private void UpdateScene(string nextScene)
    {
        currentScene = nextScene;
    }

    private void PlayMusic()
    {
        if(scenesTable[currentScene].playMusic)
            GameManager.AudioController.PlayMusic(scenesTable[currentScene].music);
    }

    public List<string> GetInitializeTags(Scene scene)
    {
        return scenesTable[scene.name].gameObjectsTag;
    }

    public void NextScene()
    {
        string nextScene = scenesTable[currentScene].nextScene.sceneName;

        if (scenesTable[currentScene].nextScene.withLoadScreen)
        {
            currentLoadScene = scenesTable[currentScene].nextScene.loadSceneName;
            sceneLoader.LoadWithLoadingScreen(nextScene, currentLoadScene);
        }
        else
            sceneLoader.LoadScene(nextScene);

        UpdateScene(nextScene);
    }

    public void PreviousScene()
    {
        string nextScene = scenesTable[currentScene].previousScene.sceneName;

        if (scenesTable[currentScene].previousScene.withLoadScreen)
        {
            currentLoadScene = scenesTable[currentScene].previousScene.loadSceneName;
            sceneLoader.LoadWithLoadingScreen(nextScene, currentLoadScene);
        }
        else
            sceneLoader.LoadScene(nextScene);

        UpdateScene(nextScene);
    }
}
