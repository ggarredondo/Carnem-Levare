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

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        SceneLoader.StartTransition += StartTransition;
        SceneLoader.EndTransition += EndTransition;
    }

    private void OnDisable()
    {
        SceneLoader.StartTransition -= StartTransition;
        SceneLoader.EndTransition -= EndTransition;
    }

    private async Task StartTransition()
    {
        GameManager.PlayerInput.enabled = false;
        GameManager.UiInput.enabled = false;
        await Lerp.Value_Unscaled(canvasGroup.alpha, 1, (a) => canvasGroup.alpha = a, lerpDuration);
        await Lerp.Value_Unscaled(childCanvasGroup.alpha, 1, (a) => childCanvasGroup.alpha = a, childLerpDuration);
        await Task.Delay(System.TimeSpan.FromSeconds(extraTime));
        GameManager.UiInput.enabled = true;
        GameManager.PlayerInput.enabled = true;
    }

    private async Task EndTransition()
    {
        GameManager.PlayerInput.enabled = false;
        GameManager.UiInput.enabled = false;
        await Lerp.Value(childCanvasGroup.alpha, 0, (a) => childCanvasGroup.alpha = a, childLerpDuration);
        await Lerp.Value(canvasGroup.alpha, 0, (a) => canvasGroup.alpha = a, lerpDuration);
        GameManager.UiInput.enabled = true;
        GameManager.PlayerInput.enabled = true;

        text.text = "";
        extraTime = 0;
    }
}
