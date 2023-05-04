using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputDetection
{
    private const float GAMEPAD_DETECTION_TIME = 0.2f;

    public int controlSchemeIndex;
    public GameObject selected;

    public System.Action controlsChangedEvent;

    public void OnControlSchemeChanged(PlayerInput playerInput)
    {
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard&Mouse":
                controlSchemeIndex = 1;
                EventSystem.current.SetSelectedGameObject(null);
                Cursor.visible = true;
                break;
            case "Gamepad":
                controlSchemeIndex = 0;
                EventSystem.current.SetSelectedGameObject(selected);
                WaitGamepadDetection(GAMEPAD_DETECTION_TIME);
                Cursor.visible = false;
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
