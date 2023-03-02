using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private AudioManager musicManager;

    private void Start()
    {
        musicManager = GameObject.FindGameObjectWithTag("MUSIC").GetComponent<AudioManager>();
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
        StartCoroutine(SceneManagement.Instance.LoadSceneByIndex(2));
    }
}
