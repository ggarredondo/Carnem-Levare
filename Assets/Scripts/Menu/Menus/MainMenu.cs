using UnityEngine;
using UnityEngine.UI;

public class MainMenu : AbstractMenu
{
    [Header("UI Elements")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private bool isLoading;

    protected override void Configure()
    {
        AudioController.Instance.PlayMusic("Intro");

        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        if (!isLoading)
        {
            AudioController.Instance.uiSfxSounds.Play("PlayGame");
            isLoading = true;
            GameManager.SceneLoader.LoadWithLoadingScreen(SceneNumber.GAME);
        }
    }
}
