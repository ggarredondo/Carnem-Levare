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
        InputDevice currentControlScheme = DetectInputDevice();

        if (currentControlScheme != previousCustomControlScheme && currentControlScheme != InputDevice.UNKNOW)
        {
            previousCustomControlScheme = currentControlScheme;
            SetCustomControlScheme();
            OnControlSchemeChanged(PlayerInput.all[0]);
        }
    }

    private InputDevice DetectInputDevice()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
            return InputDevice.KEYBOARD;
        else if (Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero)
            return InputDevice.MOUSE;
        else if (Gamepad.current != null)
            foreach (var control in Gamepad.current.allControls)
                if (control is ButtonControl button && button.isPressed)
                    return InputDevice.GAMEPAD;

        return InputDevice.UNKNOW;
    }

    private void SetCustomControlScheme()
    {
        if (PlayerInput.all.Count > 0)
        {
            if (previousCustomControlScheme == InputDevice.KEYBOARD)
            {
                PlayerInput.all[0].SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);

                if (Cursor.visible == true)
                {
                    EventSystem.current.SetSelectedGameObject(selected);
                    Cursor.visible = false;
                }
            }
            else if (previousCustomControlScheme == InputDevice.MOUSE)
            {
                if (PlayerInput.all[0].currentActionMap.name != "Main Movement")
                    PlayerInput.all[0].SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);

                EventSystem.current.SetSelectedGameObject(null);
                Cursor.visible = true;
            }
            else if (previousCustomControlScheme == InputDevice.GAMEPAD)
            {
                PlayerInput.all[0].SwitchCurrentControlScheme("Gamepad", Gamepad.current);

                if (Cursor.visible == true)
                {
                    EventSystem.current.SetSelectedGameObject(selected);
                    GameManager.AudioController.Play("SelectButton");
                    WaitGamepadDetection(GAMEPAD_DETECTION_TIME);
                    Cursor.visible = false;
                }  
            }
        }
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
        SetCustomControlScheme();
        OnControlSchemeChanged(PlayerInput.all[0]);
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
