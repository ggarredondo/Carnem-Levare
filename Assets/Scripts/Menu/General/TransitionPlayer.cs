using System.Threading.Tasks;
using UnityEngine;
using LerpUtilities;

public class TransitionPlayer : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] private float lerpDuration;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
        GameManager.UiInput.enabled = true;
        GameManager.PlayerInput.enabled = true;
    }

    private async Task EndTransition()
    {
        GameManager.PlayerInput.enabled = false;
        GameManager.UiInput.enabled = false;
        await Lerp.CanvasAlpha(canvasGroup, 0, lerpDuration);
        GameManager.UiInput.enabled = true;
        GameManager.PlayerInput.enabled = true;
    }
}
