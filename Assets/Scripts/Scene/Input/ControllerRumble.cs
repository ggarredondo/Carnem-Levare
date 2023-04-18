using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerRumble : Singleton<ControllerRumble>
{
    private Gamepad gamepad;
    private bool isRumbling;

    private void Start()
    {
        gamepad = Gamepad.current;
    }

    public void Rumble(float duration, float leftAmplitude, float rightAmplitude)
    {
        if (gamepad != null && SceneManagement.Instance.PlayerInput.currentControlScheme == "Gamepad" && !isRumbling && DataSaver.options.rumble)
        {
            gamepad.SetMotorSpeeds(leftAmplitude, rightAmplitude);
            isRumbling = true;
            StartCoroutine(StopRumble(duration));
        }
    }

    private IEnumerator StopRumble(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        gamepad.SetMotorSpeeds(0f, 0f);
        isRumbling = false;
    }
}