using System.Threading.Tasks;
using UnityEngine;
using LerpUtilities;
using TMPro;

public class TransitionPlayer : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup childCanvasGroup;

    [Header("Parameters")]
    [SerializeField] private float lerpDuration;
    [SerializeField] private float childLerpDuration;

    public static TMP_Text text;
    public static float extraTime;

    public void Initialize()
    {
        SceneLoader.StartTransition += StartTransition;
        SceneLoader.EndTransition += EndTransition;
    }

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void OnDestroy()
    {
        SceneLoader.StartTransition -= StartTransition;
        SceneLoader.EndTransition -= EndTransition;
    }

    private async Task StartTransition()
    {
        GameManager.InputUtilities.ActivatePlayerInput(false);
        GameManager.InputUtilities.EnableUIModule(false);
        await Lerp.Value_Unscaled(canvasGroup.alpha, 1, (a) => canvasGroup.alpha = a, lerpDuration);
        await Lerp.Value_Unscaled(childCanvasGroup.alpha, 1, (a) => childCanvasGroup.alpha = a, childLerpDuration);
        await Task.Delay(System.TimeSpan.FromSeconds(extraTime));
    }

    private async Task EndTransition()
    {
        GameManager.InputUtilities.ActivatePlayerInput(false);
        GameManager.InputUtilities.EnableUIModule(false);
        await Lerp.Value(childCanvasGroup.alpha, 0, (a) => childCanvasGroup.alpha = a, childLerpDuration);
        await Lerp.Value(canvasGroup.alpha, 0, (a) => canvasGroup.alpha = a, lerpDuration);
        GameManager.InputUtilities.EnableUIModule(true);
        GameManager.InputUtilities.ActivatePlayerInput(true);

        text.text = "";
        extraTime = 0;
    }
}
