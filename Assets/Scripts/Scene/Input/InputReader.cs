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
    public event UnityAction<bool> Attack0Event, Attack1Event, 
        Attack2Event, Attack3Event;

    #region UI
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
    #endregion

    #region Main Movement

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementEvent?.Invoke(context.ReadValue<Vector2>());
    }
    public void OnBlock(InputAction.CallbackContext context)
    {
        BlockEvent.Invoke(context.performed);
    }

    public void OnAttack0(InputAction.CallbackContext context)
    {
        Attack0Event.Invoke(context.performed);
    }
    public void OnAttack1(InputAction.CallbackContext context)
    {
        Attack1Event.Invoke(context.performed);
    }

    public void OnAttack2(InputAction.CallbackContext context)
    {
        Attack2Event.Invoke(context.performed);
    }
    public void OnAttack3(InputAction.CallbackContext context)
    {
        Attack3Event.Invoke(context.performed);
    }

    #endregion
}
