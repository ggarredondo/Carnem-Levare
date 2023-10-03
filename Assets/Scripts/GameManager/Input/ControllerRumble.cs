using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class ControllerRumble
{
    private Gamepad gamepad;
    private bool isRumbling;

    public ControllerRumble()
    {
        gamepad = Gamepad.current;
    }

    public void Rumble(float duration, float leftAmplitude, float rightAmplitude)
    {
        if (gamepad != null && GameManager.Input.CurrentControlScheme() == "Gamepad" && !isRumbling && DataSaver.Options.rumble)
        {
            gamepad.SetMotorSpeeds(leftAmplitude, rightAmplitude);
            isRumbling = true;
            StopRumble(duration);
        }
    }

    private async void StopRumble(float duration)
    {
        await Task.Delay(System.TimeSpan.FromSeconds(duration));
        gamepad.SetMotorSpeeds(0f, 0f);
        isRumbling = false;
    }
}