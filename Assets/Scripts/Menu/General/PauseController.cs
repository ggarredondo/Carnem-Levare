using UnityEngine;
using UnityEngine.Rendering;

public class PauseController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private MenuController menuController;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject volume;

    [Header("Parameters")]
    [SerializeField] [Range(0f, 1f)] private float slowMotion;

    private bool pauseMenuActivated = false;

    private void Awake()
    {
        menuController.pauseMenu = true;
    }

    protected void OnEnable()
    {
        menuController.ExitPauseMenuEvent += EnterPauseMenu;
        inputReader.StartPauseMenuEvent += EnterPauseMenu;
    }

    protected void OnDisable()
    {
        menuController.ExitPauseMenuEvent -= EnterPauseMenu;
        inputReader.StartPauseMenuEvent -= EnterPauseMenu;
    }

    private void EnterPauseMenu()
    {
        if (!pauseMenuActivated) EnterPauseMode();
        else ExitPauseMode(true);
    }

    public void EnterPauseMode()
    {
        Time.timeScale = slowMotion;
        menuController.tree.Initialize();
        pauseMenuActivated = true;
        volume.SetActive(true);
        GameManager.PlayerInput.SwitchCurrentActionMap("UI");
        AudioController.Instance.PauseGame(true);
    }

    public void ExitPauseMode(bool resumeSounds)
    {
        Time.timeScale = 1;
        menuController.DisableMenus();
        pauseMenuActivated = false;
        volume.SetActive(false);
        GameManager.PlayerInput.SwitchCurrentActionMap("Main Movement");
        AudioController.Instance.PauseGame(false && resumeSounds);
    }
}
