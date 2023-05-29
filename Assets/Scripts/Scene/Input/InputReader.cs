using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/Input Reader")]
public class InputReader : ScriptableObject
{
    public event System.Action MouseClickEvent;
    public event System.Action MenuBackEvent;
    public event System.Action ChangeRightMenuEvent;
    public event System.Action ChangeLeftMenuEvent;
    public event System.Action StartPauseMenuEvent;

    public event System.Action SelectMenuEvent;
    public event System.Action ChangeCamera;
    public event System.Action<Vector2> DPAdEvent;

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        MouseClickEvent?.Invoke();
    }

    public void OnMenuBack(InputAction.CallbackContext context)
    {
        if (context.performed) MenuBackEvent.Invoke();
    }

    public void OnChangeRightMenu(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeRightMenuEvent?.Invoke();
    }

    public void OnChangeLeftMenu(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeLeftMenuEvent?.Invoke(); 
    }

    public void OnStartPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed) StartPauseMenuEvent?.Invoke();
    }

    public void OnSelectMenu(InputAction.CallbackContext context)
    {
        if (context.performed) SelectMenuEvent?.Invoke();
    }

    public void OnDPAd(InputAction.CallbackContext context)
    {
        if (context.performed) DPAdEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnChangeCamera(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeCamera?.Invoke();
    }
}
