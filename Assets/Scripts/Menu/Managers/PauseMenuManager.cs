using UnityEngine;

public class PauseMenuManager : MainMenuManager
{
    [Range(0f, 1f)] public float slowMotion;
    private bool pauseMenuActivated = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        inputReader.StartPauseMenuEvent += EnterPauseMenu;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        inputReader.StartPauseMenuEvent -= EnterPauseMenu;
    }

    protected override void Awake()
    {
        base.Awake();

        DisableActiveMenu();
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic("Fight");
    }

    public void ReturnMainMenu()
    {
        ExitPauseMode(false);
        AudioManager.Instance.BackMenu();
        GameManager.SceneLoader.LoadScene((int) SceneNumber.NON_DESTROY_MAIN_MENU);
    }

    public void ResumeGame()
    {
        ExitPauseMode(true);
    }

    private void EnterPauseMenu()
    {
        if (!pauseMenuActivated) EnterPauseMode();
        else ExitPauseMode(true);
    }

    protected override void ReturnFromChildren()
    {
        if (actualActiveMenu == 0) ExitPauseMode(true);

        base.ReturnFromChildren();
    }

    private void EnterPauseMode()
    {
        Time.timeScale = slowMotion;
        ChangeMenu(firstMenu);
        pauseMenuActivated = true;
        GameManager.PlayerInput.SwitchCurrentActionMap("UI");
        AudioManager.Instance.PauseGame(true);
    }

    private void ExitPauseMode(bool resumeSounds)
    {
        Time.timeScale = 1;
        DisableActiveMenu();
        pauseMenuActivated = false;
        GameManager.PlayerInput.SwitchCurrentActionMap("Main Movement");
        AudioManager.Instance.PauseGame(false && resumeSounds);
    }
}
