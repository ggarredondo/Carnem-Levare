using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PauseMenuManager : MainMenuManager
{
    [Range(0f, 1f)] public float slowMotion;
    public PlayerInput playerController;

    private AudioSaver audioMixer;
    private AudioManager musicManager;
    private AudioManager sfxManager;
    private bool pauseMenuActivated = false;

    protected override void Awake()
    {
        base.Awake();

        DisableActiveMenu();

        playerController.uiInputModule = GameObject.FindGameObjectWithTag("UI").GetComponent<InputSystemUIInputModule>();
        audioMixer = GameObject.FindGameObjectWithTag("AUDIO").GetComponent<AudioSaver>();
        musicManager = GameObject.FindGameObjectWithTag("MUSIC").GetComponent<AudioManager>();
        sfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();

        musicManager.Play("Fight");
    }

    /// <summary>
    /// enable or disable the pause menu
    /// </summary>
    /// <param name="context"></param>
    public void EnterPauseMenu(InputAction.CallbackContext context)
    {
        if(context.performed)
            if (!pauseMenuActivated)
            {
                EnterPauseMode();
            }
            else
            {
                ExitPauseMode(true);
            }
    }

    /// <summary>
    /// All the options to enable when enter pause menu
    /// </summary>
    private void EnterPauseMode()
    {
        Time.timeScale = slowMotion;
        ChangeMenu(firstMenu);
        pauseMenuActivated = true;
        playerController.SwitchCurrentActionMap("UI");
        sfxManager.PauseAllSounds();
    }

    /// <summary>
    /// All the options to disble when exit pause menu
    /// </summary>
    private void ExitPauseMode(bool resumeSounds)
    {
        Time.timeScale = 1;
        DisableActiveMenu();
        pauseMenuActivated = false;
        playerController.SwitchCurrentActionMap("Main Movement");

        if (resumeSounds)
            sfxManager.ResumeAllSounds();
    }

    /// <summary>
    /// Quit the game, exit to the desk
    /// </summary>
    public void ReturnMainMenu()
    {
        ExitPauseMode(false);
        SceneManagement.LoadPreviousScene();
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
        ExitPauseMode(true);
    }
}
