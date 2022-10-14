using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class CustomTapInteraction : IInputInteraction
{
    private const float durationDefault = 0.2f, pressPointDefault = 0.5f;

    public float duration = durationDefault; // The time in seconds within which the control needs to be pressed and released to perform the interaction.
    public float pressPoint = pressPointDefault; // The press point required to perform the interaction.
    
    private float durationOrDefault => duration > 0f ? duration : durationDefault; // If either value is zero or less, use defaults.
    private float pressPointOrDefault => pressPoint > 0f ? pressPoint : pressPointDefault;

    private double m_TapStartTime = 0.0;

    /// <summary>
    /// Static constructor to register the interaction and make it available in the Input Action Asset Editor window.
    /// </summary>
    static CustomTapInteraction() { InputSystem.RegisterInteraction<CustomTapInteraction>(); }

    /// <summary>
    /// Called when the game loads. Will also execute static constructor.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize() {}

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
                if (context.ReadValue<float>() >= pressPointOrDefault)
                {
                    m_TapStartTime = context.time;
                    context.Started();
                    // Set timeout slightly after duration so that if tap comes in exactly at the expiration
                    // time, it still counts as a valid tap.
                    context.SetTimeout(duration + 0.00001f);
                }
                break;

            case InputActionPhase.Started:
                if (context.time - m_TapStartTime <= durationOrDefault && context.ReadValue<float>() <= 0f)
                    context.PerformedAndStayPerformed();
                else
                    context.Canceled();
                break;

            case InputActionPhase.Performed:
                context.Canceled();
                break;
        }
    }

    public void Reset() { m_TapStartTime = 0.0; }
}