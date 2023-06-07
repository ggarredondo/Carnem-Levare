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

    public event System.Action StartHoldEvent;
    public event System.Action StartHoldInitEvent;

    public event System.Action SelectMenuEvent;
    public event System.Action ChangeCamera;
    public event System.Action<Vector2> DPAdEvent;

    public event System.Action<int> Action0;
    public event System.Action<int> Action1;
    public event System.Action<int> Action2;
    public event System.Action<int> Action3;

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

    public void OnStartHold(InputAction.CallbackContext context)
    {
        if (context.started) StartHoldInitEvent?.Invoke();
        if (context.performed) StartHoldEvent?.Invoke();
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

    public void OnAction0(InputAction.CallbackContext context)
    {
        if (context.performed) Action0?.Invoke(0);
    }

    public void OnAction1(InputAction.CallbackContext context)
    {
        if (context.performed) Action1?.Invoke(1);
    }
    public void OnAction2(InputAction.CallbackContext context)
    {
        if (context.performed) Action2?.Invoke(2);
    }
    public void OnAction3(InputAction.CallbackContext context)
    {
        if (context.performed) Action3?.Invoke(3);
    }
}
