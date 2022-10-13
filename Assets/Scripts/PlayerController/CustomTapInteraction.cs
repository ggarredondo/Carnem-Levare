using UnityEngine.InputSystem;
//#if UNITY_EDITOR
//using UnityEngine.InputSystem.Editor;
//#endif

public class CustomTapInteraction : IInputInteraction
{
    private const float durationDefault = 0.2f, pressPointDefault = 0.5f;

    /// <summary>
    /// The time in seconds within which the control needs to be pressed and released to perform the interaction.
    /// </summary>
    public float duration = durationDefault;

    /// <summary>
    /// The press point required to perform the interaction.
    /// </summary>
    public float pressPoint = pressPointDefault;

    // The system will use the previously defined defaults in case either value is zero or less.
    private float durationOrDefault => duration > 0f ? duration : durationDefault;
    private float pressPointOrDefault => pressPoint > 0f ? pressPoint : pressPointDefault;

    private double m_TapStartTime = 0.0;

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
                    context.Performed();
                else
                    context.Canceled();
                break;

            //case InputActionPhase.Performed:
            //    context.Canceled();
            //    break;
        }
    }

    public void Reset() { m_TapStartTime = 0.0; }
}

//#if UNITY_EDITOR
//internal class CustomTapInteractionEditor : InputParameterEditor<CustomTapInteraction>
//{
//    protected override void OnEnable() { InputSystem.RegisterInteraction<CustomTapInteraction>(); }
//    public override void OnGUI() { }
//}
//#endif
