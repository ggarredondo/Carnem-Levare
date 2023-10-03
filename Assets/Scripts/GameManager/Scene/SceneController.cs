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

        sceneLoader = new();
        scenesTable = new();

        currentScene = initialScene;

        this.scenes = scenes;
        Initialize();
    }

    private void Initialize()
    {
        scenes.ForEach(s => scenesTable.Add(s.sceneName, s));
    }

    private void UpdateScene(string nextScene)
    {
        currentScene = nextScene;
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

    public string GetCurrentLoadScene()
    {
        return currentLoadScene;
    }

    public SceneLogic GetSceneLogic(string name)
    {
        return scenesTable[currentScene];
    }
}
