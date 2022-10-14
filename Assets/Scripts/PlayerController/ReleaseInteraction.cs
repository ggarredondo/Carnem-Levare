using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class ReleaseInteraction : IInputInteraction
{
    private const float timeoutDefault = 0.2f;
    public float timeout = timeoutDefault; // The time in seconds within which the control needs to be pressed and released to perform the interaction.
    private float timeoutOrDefault => timeout > 0f ? timeout : timeoutDefault; // If value is zero or less, use defaults.

    /// <summary>
    /// Static constructor to register the interaction and make it available in the Input Action Asset Editor window.
    /// </summary>
    static ReleaseInteraction() { InputSystem.RegisterInteraction<ReleaseInteraction>(); }

    /// <summary>
    /// Called when the game loads. Will also execute static constructor.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize() { }

    public void Process(ref InputInteractionContext context)
    {
        if (context.timerHasExpired)
        {
            context.Canceled();
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                if (context.ReadValue<float>() >= 1)
                {
                    context.Started();
                    context.SetTimeout(timeoutOrDefault);
                }
                break;

            case InputActionPhase.Started:
                if (context.ReadValue<float>() <= 0)
                    context.Performed();
                break;
        }
    }

    public void Reset() { }
}