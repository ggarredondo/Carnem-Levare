
using UnityEngine.InputSystem;

public class CustomTapInteraction : IInputInteraction
{
    public float duration = 0.2f;

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
                }
                break;

            case InputActionPhase.Started:
                break;

            case InputActionPhase.Performed:
                break;
        }
    }

    public void Reset()
    {

    }
}
