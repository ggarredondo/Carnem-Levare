using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController
{
    private Dictionary<string, SceneLogic> scenesTable;
    private List<SceneLogic> scenes;
    private SceneLoader sceneLoader;

    private string currentScene;
    private string currentLoadScene;

    public SceneController(string initialScene, List<SceneLogic> scenes)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        sceneLoader = new();
        scenesTable = new();

        currentScene = initialScene;

        this.scenes = scenes;
        Initialize();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name != currentLoadScene)
        {
            AudioController.Instance.InitializeSoundSources(GetCurrentSounds());
            PlayMusic();
        }
    }

    private void Initialize()
    {
        scenes.ForEach(s => scenesTable.Add(s.sceneObject.name, s));
    }

    private void UpdateScene(string nextScene)
    {
        currentScene = nextScene;
    }

    public void PlayMusic()
    {
        if(scenesTable[currentScene].playMusic)
            AudioController.Instance.PlayMusic(scenesTable[currentScene].music);
    }

    public List<Sounds> GetCurrentSounds()
    {
        return scenesTable[currentScene].sounds;
    }

    public void NextScene()
    {
        string nextScene = scenesTable[currentScene].nextScene.sceneObject.name;

        if (scenesTable[currentScene].nextScene.withLoadScreen)
        {
            currentLoadScene = scenesTable[currentScene].nextScene.loadSceneObject.name;
            sceneLoader.LoadWithLoadingScreen(nextScene, currentLoadScene);
        }
        else
            sceneLoader.LoadScene(nextScene);

        UpdateScene(nextScene);
    }

    public void PreviousScene()
    {
        string nextScene = scenesTable[currentScene].previousScene.sceneObject.name;

        if (scenesTable[currentScene].previousScene.withLoadScreen)
        {
            currentLoadScene = scenesTable[currentScene].previousScene.loadSceneObject.name;
            sceneLoader.LoadWithLoadingScreen(nextScene, currentLoadScene);
        }
        else
            sceneLoader.LoadScene(nextScene);

        UpdateScene(nextScene);
    }
}
