using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputDetection
{
    private const float GAMEPAD_DETECTION_TIME = 0.2f;

    public int controlSchemeIndex;
    public GameObject selected;

    public System.Action controlsChangedEvent;

    public InputDevice previousCustomControlScheme = InputDevice.UNKNOW;

    public void CheckCustomControlScheme()
    {
        InputDevice currentControlScheme = GetCustomControlScheme();

        if (currentControlScheme != previousCustomControlScheme && currentControlScheme != InputDevice.UNKNOW)
            previousCustomControlScheme = currentControlScheme;
    }

    private InputDevice GetCustomControlScheme()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
        {
            if (Cursor.visible == true)
            {
                EventSystem.current.SetSelectedGameObject(selected);
                WaitGamepadDetection(GAMEPAD_DETECTION_TIME);
                Cursor.visible = false;
            }

            return InputDevice.KEYBOARD;
        }
        else if (Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Cursor.visible = true;
            return InputDevice.MOUSE;
        }
        else if (Gamepad.current != null)
        {
            foreach (var control in Gamepad.current.allControls)
            {
                if (control is ButtonControl button && button.isPressed)
                {
                    if (Cursor.visible == true)
                    {
                        EventSystem.current.SetSelectedGameObject(selected);
                        WaitGamepadDetection(GAMEPAD_DETECTION_TIME);
                        Cursor.visible = false;
                    }

                    return InputDevice.GAMEPAD;
                }
            }
        }

        return InputDevice.UNKNOW;
    }

    public void OnControlSchemeChanged(PlayerInput playerInput)
    {
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard&Mouse":
                controlSchemeIndex = 1;
                break;
            case "Gamepad":
                controlSchemeIndex = 0;
                break;
        }

        controlsChangedEvent?.Invoke();
    }

    public void Configure()
    {
        GameManager.PlayerInput.controlsChangedEvent.AddListener(OnControlSchemeChanged);
        OnControlSchemeChanged(GameManager.PlayerInput);
    }

    private async void WaitGamepadDetection(float time)
    {
        GameManager.UiInput.enabled = false;
        await Task.Delay(System.TimeSpan.FromSeconds(time));
        GameManager.UiInput.enabled = true;
    }
}

public enum InputDevice
{
    GAMEPAD,
    KEYBOARD,
    MOUSE,
    UNKNOW
}
