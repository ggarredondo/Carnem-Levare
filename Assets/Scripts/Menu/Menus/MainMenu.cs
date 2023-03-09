using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private AudioManager musicManager;
    private bool isLoading;

    private void Start()
    {
        musicManager = GameObject.FindGameObjectWithTag("MUSIC").GetComponent<AudioManager>();
        musicManager.StopAllSounds();
        musicManager.Play("Intro");
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
            SoundEvents.Instance.PlayGame.Invoke();
            isLoading = true;
            StartCoroutine(SceneManagement.Instance.LoadSceneByIndexAsync(3));
        }
    }
}
