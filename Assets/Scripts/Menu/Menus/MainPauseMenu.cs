using UnityEngine;
using UnityEngine.UI;

public class MainPauseMenu : AbstractMenu
{
    [SerializeField] private PauseController pauseController;

    [Header("UI Elements")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button backButton;

    protected override void Configure()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        backButton.onClick.AddListener(BackToMenu);
    }

    private void ResumeGame()
    {
        pauseController.ExitPauseMode(true);
    }

    private void BackToMenu()
    {
        AudioController.Instance.BackMenu();
        GameManager.SceneController.PreviousScene();
    }
}
