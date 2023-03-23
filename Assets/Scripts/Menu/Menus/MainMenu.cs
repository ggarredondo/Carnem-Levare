using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool isLoading;

    private void Start()
    {
        AudioManager.Instance.PlayMusic("Intro");
    }

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
            StartCoroutine(SceneManagement.Instance.LoadSceneByIndexAsync(3));
        }
    }
}
