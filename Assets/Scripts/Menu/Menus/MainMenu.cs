using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool isLoading;

    private void Start()
    {
        AudioManager.Instance.PlayMusic("Intro");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        if (!isLoading)
        {
            AudioManager.Instance.uiSfxSounds.Play("PlayGame");
            isLoading = true;
            GameManager.SceneLoader.LoadWithLoadingScreen(SceneNumber.GAME);
        }
    }
}
