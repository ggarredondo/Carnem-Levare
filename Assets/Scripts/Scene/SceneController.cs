using System.Collections.Generic;

public class SceneController
{
    private Dictionary<int, SceneLogic> scenesTable;
    private List<SceneLogic> scenes;
    private SceneLoader sceneLoader;
    private int currentScene;

    public SceneController(int initialScene, List<SceneLogic> scenes)
    {
        sceneLoader = new();
        scenesTable = new();

        currentScene = initialScene;

        this.scenes = scenes;
        Initialize();
    }

    private void Initialize()
    {
        scenes.ForEach(s => scenesTable.Add((int)s.ID, s));
    }

    private void UpdateScene(int nextScene)
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
        SceneNumber nextScene = scenesTable[currentScene].nextScene.ID;

        if (scenesTable[currentScene].nextScene.withLoadScreen)
            sceneLoader.LoadWithLoadingScreen(nextScene);
        else
            sceneLoader.LoadScene(nextScene);

        UpdateScene((int)nextScene);
    }

    public void PreviousScene()
    {
        SceneNumber nextScene = scenesTable[currentScene].previousScene.ID;

        if (scenesTable[currentScene].previousScene.withLoadScreen)
            sceneLoader.LoadWithLoadingScreen(nextScene);
        else
            sceneLoader.LoadScene(nextScene);

        UpdateScene((int)nextScene);
    }
}

public enum SceneNumber
{
    MAIN_MENU = 0,
    LOADING = 1,
    NON_DESTROY_MAIN_MENU = 2,
    COMBAT = 3,
    TRAINING = 4,
    PHOTOSHOP = 5
}
