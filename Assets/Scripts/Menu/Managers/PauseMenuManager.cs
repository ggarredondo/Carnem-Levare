using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PauseMenuManager : MainMenuManager
{
    [Range(0f, 1f)] public float slowMotion;
    private PlayerInput playerInput;

    private AudioManager sfxManager;
    private bool pauseMenuActivated = false;

    protected override void Awake()
    {
        base.Awake();

        DisableActiveMenu();
    }

    private void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();
        playerInput.uiInputModule = GameObject.FindGameObjectWithTag("UI").GetComponent<InputSystemUIInputModule>();
        sfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();

        SoundEvents.Instance.PlayMusic("Fight");
    }

    /// <summary>
    /// enable or disable the pause menu
    /// </summary>
    /// <param name="context"></param>
    public void EnterPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
            if (!pauseMenuActivated)
            {
                EnterPauseMode();
            }
            else
            {
                ExitPauseMode(true);
            }
        
    }

    public override void ReturnFromChildren(InputAction.CallbackContext context)
    {
        if (context.performed && actualActiveMenu == 0)
        {
            ExitPauseMode(true);
        }

        base.ReturnFromChildren(context);
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
            sfxManager.PauseAllSounds();
            SoundEvents.Instance.PauseGame.Invoke();
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
        SoundEvents.Instance.PauseGame.Invoke();
        if (resumeSounds)
            sfxManager.ResumeAllSounds();
    }

    /// <summary>
    /// Quit the game, exit to the desk
    /// </summary>
    public void ReturnMainMenu()
    {
        ExitPauseMode(false);
        sfxManager.StopAllSounds();
        SoundEvents.Instance.BackMenu.Invoke();
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
