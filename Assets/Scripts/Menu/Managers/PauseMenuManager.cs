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
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();
        playerInput.uiInputModule = GameObject.FindGameObjectWithTag("UI").GetComponent<InputSystemUIInputModule>();

        SoundEvents.Instance.PlayMusic("Fight");
    }

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
            playerInput.SwitchCurrentActionMap("UI");
            SoundEvents.Instance.PauseGame(true);
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
        playerInput.SwitchCurrentActionMap("Main Movement");
        SoundEvents.Instance.PauseGame(false && resumeSounds);
    }

    /// <summary>
    /// Quit the game, exit to the desk
    /// </summary>
    public void ReturnMainMenu()
    {
        ExitPauseMode(false);
        SoundEvents.Instance.BackMenu();
        StartCoroutine(SceneManagement.Instance.LoadPreviousScene());
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
        ExitPauseMode(true);
    }
}
