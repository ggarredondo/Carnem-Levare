public interface IChangeScene
{
    public void NextScene();
    public void PreviousScene();
    public string GetCurrentLoadScene();
    public SceneLogic GetSceneLogic(string name);
}
