using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PauseMenuManager : MainMenuManager
{
    [Range(0f, 1f)] public float slowMotion;
    private PlayerInput playerInput;

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

    #region Public

    /// <summary>
    /// Quit the game, exit to the desk
    /// </summary>
    public void ReturnMainMenu()
    {
        ExitPauseMode(false);
        AudioManager.Instance.BackMenu();
        StartCoroutine(GameManager.SceneLoader.LoadScene((int) SceneNumber.NON_DESTROY_MAIN_MENU));
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
        ExitPauseMode(true);
    }

    #endregion

    #region Private

    /// <summary>
    /// enable or disable the pause menu
    /// </summary>
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

    /// <summary>
    /// All the options to enable when enter pause menu
    /// </summary>
    private void EnterPauseMode()
    {
        if (SceneManagement.Instance.TransitionEnd)
        {
            Time.timeScale = slowMotion;
            ChangeMenu(firstMenu);
            pauseMenuActivated = true;
            GameManager.PlayerInput.SwitchCurrentActionMap("UI");
            AudioManager.Instance.PauseGame(true);
        }
    }

    /// <summary>
    /// All the options to disble when exit pause menu
    /// </summary>
    private void ExitPauseMode(bool resumeSounds)
    {
        Time.timeScale = 1;
        DisableActiveMenu();
        pauseMenuActivated = false;
        GameManager.PlayerInput.SwitchCurrentActionMap("Main Movement");
        AudioManager.Instance.PauseGame(false && resumeSounds);
    }

    #endregion
}
