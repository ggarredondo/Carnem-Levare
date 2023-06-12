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
        await Lerp.CanvasAlpha_Unscaled(canvasGroup, 1, lerpDuration);
        await Lerp.CanvasAlpha_Unscaled(childCanvasGroup, 1, childLerpDuration);
        await Task.Delay(System.TimeSpan.FromSeconds(extraTime));
        GameManager.UiInput.enabled = true;
        GameManager.PlayerInput.enabled = true;
    }

    private async Task EndTransition()
    {
        GameManager.PlayerInput.enabled = false;
        GameManager.UiInput.enabled = false;
        await Lerp.CanvasAlpha(childCanvasGroup, 0, childLerpDuration);
        await Lerp.CanvasAlpha(canvasGroup, 0, lerpDuration);
        GameManager.UiInput.enabled = true;
        GameManager.PlayerInput.enabled = true;

        text.text = "";
        extraTime = 0;
    }
}
