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
}
