using UnityEngine;
using TMPro;
using LerpUtilities;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class HoldText : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private InputActionReference inputReference;
    [SerializeField] private TMP_Text tmpText;

    [Header("Parameters")]
    [SerializeField] private float minHoldTime;
    [SerializeField] private float holdTIme;

    private CancellationTokenSource cancellationTokenSource;
    private bool hasTrigger;

    private void Start()
    {
        inputReader.HoldEvents[inputReference.action] += Hold;
    }

    private void OnDestroy()
    {
        inputReader.HoldEvents[inputReference.action] -= Hold;
    }

    private void Hold(InputAction.CallbackContext context)
    {
        if (context.started) StartHolding();
        if (context.performed) TriggerHold();
        if (context.canceled && !hasTrigger) ReleaseHold();
    }

    private void ReleaseHold()
    {
        cancellationTokenSource?.Cancel();
        tmpText.color = new Color(1, 1, 1);
    }

    private async void StartHolding()
    {
        cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        try
        {
            await Task.Delay(System.TimeSpan.FromSeconds(minHoldTime));
            await Lerp.Value_Cancel(tmpText.color, new Color(1, 0, 0),(c) => tmpText.color = c , holdTIme, cancellationToken);
        }
        catch (TaskCanceledException) { }
        finally
        {
            cancellationTokenSource.Dispose();
        }
    }

    private void TriggerHold()
    {
        hasTrigger = true;
        GameManager.AudioController.Play("PlayGame");
        GameManager.SceneController.NextScene();
    }
}
