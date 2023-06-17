using UnityEngine;
using LerpUtilities;

public class PauseController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private MenuController menuController;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject volume;

    [Header("Parameters")]
    [SerializeField] [Range(0f, 1f)] private float slowMotion;

    private bool pauseMenuActivated = false;
    private CanvasGroup canvasGroup;

    public static System.Action EnterPause;
    public static System.Action ExitPause;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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

    public async void EnterPauseMode()
    {
        Time.timeScale = slowMotion;
        menuController.tree.Initialize();
        pauseMenuActivated = true;
        volume.SetActive(true);
        EnterPause?.Invoke();
        GameManager.PlayerInput.SwitchCurrentActionMap("UI");
        AudioController.Instance.PauseGame(true);
        await Lerp.Value_Unscaled(canvasGroup.alpha, 1,(a) => canvasGroup.alpha = a, 0.1f);
    }

    public async void ExitPauseMode(bool resumeSounds)
    {
        Time.timeScale = 1;
        AudioController.Instance.PauseGame(false && resumeSounds);
        GameManager.PlayerInput.SwitchCurrentActionMap("Main Movement");
        await Lerp.Value_Unscaled(canvasGroup.alpha, 0, (a) => canvasGroup.alpha = a, 0.1f);
        menuController.DisableMenus();
        pauseMenuActivated = false;
        volume.SetActive(false);
        ExitPause?.Invoke();
    }
}
