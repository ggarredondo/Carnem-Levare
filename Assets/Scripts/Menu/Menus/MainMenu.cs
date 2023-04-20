using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool isLoading;

    private void Start()
    {
        AudioManager.Instance.PlayMusic("Intro");
    }

    #region Public

    /// <summary>
    /// Quit the game, exit to the desk
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Start the game
    /// </summary>
    public void PlayGame()
    {
        if (!isLoading)
        {
            AudioManager.Instance.uiSfxSounds.Play("PlayGame");
            isLoading = true;
            StartCoroutine(GameManager.SceneLoader.LoadWithLoadingScreen((int) SceneNumber.GAME));
        }
    }

    #endregion
}
