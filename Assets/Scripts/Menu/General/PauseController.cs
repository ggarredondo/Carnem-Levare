using System.Threading.Tasks;
using UnityEngine;

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
        EnterPause.Invoke();
        GameManager.PlayerInput.SwitchCurrentActionMap("UI");
        AudioController.Instance.PauseGame(true);
        await LerpCanvasAlpha(canvasGroup, 1, 0.1f);
    }

    public async void ExitPauseMode(bool resumeSounds)
    {
        Time.timeScale = 1;
        AudioController.Instance.PauseGame(false && resumeSounds);
        await LerpCanvasAlpha(canvasGroup, 0, 0.1f);
        menuController.DisableMenus();
        pauseMenuActivated = false;
        volume.SetActive(false);
        ExitPause.Invoke();
        GameManager.PlayerInput.SwitchCurrentActionMap("Main Movement");
    }

    private async Task LerpCanvasAlpha(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            await Task.Yield();
        }

        canvasGroup.alpha = targetAlpha;
    }
}
