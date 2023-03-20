using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/Input Reader")]
public class InputReader : ScriptableObject
{
    // UI
    public event UnityAction MouseClickEvent;
    public event UnityAction MenuBackEvent;
    public event UnityAction ChangeRightMenuEvent;
    public event UnityAction ChangeLeftMenuEvent;
    public event UnityAction StartPauseMenuEvent;

    // Main Movement
    public event UnityAction<Vector2> MovementEvent;
    public event UnityAction<bool> BlockEvent;
    public event UnityAction<bool> Left0Event, Left1Event;
    public event UnityAction<bool> Right0Event, Right1Event;

    #region UI
    public void OnMouseClick(InputAction.CallbackContext context)
    {
        MouseClickEvent.Invoke();
    }

    public void OnMenuBack(InputAction.CallbackContext context)
    {
        if (context.performed) MenuBackEvent.Invoke();
    }

    public void OnChangeRightMenu(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeRightMenuEvent.Invoke();
    }

    public void OnChangeLeftMenu(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeLeftMenuEvent.Invoke();
    }

    public void OnStartPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed) StartPauseMenuEvent.Invoke();
    }
    #endregion

    #region Main Movement

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementEvent.Invoke(context.ReadValue<Vector2>());
    }
    public void OnBlock(InputAction.CallbackContext context)
    {
        BlockEvent.Invoke(context.performed);
    }

    public void OnLeft0(InputAction.CallbackContext context)
    {
        Left0Event.Invoke(context.performed);
    }
    public void OnLeft1(InputAction.CallbackContext context)
    {
        Left1Event.Invoke(context.performed);
    }

    public void OnRight0(InputAction.CallbackContext context)
    {
        Right0Event.Invoke(context.performed);
    }
    public void OnRight1(InputAction.CallbackContext context)
    {
        Right1Event.Invoke(context.performed);
    }

    #endregion
}
