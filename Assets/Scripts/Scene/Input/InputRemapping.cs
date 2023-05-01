using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class InputRemapping
{
    private InputAction action, originalAction;

    public void Remapping(float rebindTimeDelay)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");

        action = GameManager.PlayerInput.actions.FindAction(EventSystem.current.currentSelectedGameObject.name);

        if (action == null)
            Debug.Log("This action not exists");
        else
        {
            //globalMenuManager.PopUpMessage("Waiting for input");

            originalAction = action.Clone();

            action.PerformInteractiveRebinding(GameManager.InputDetection.controlSchemeIndex)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(rebindTimeDelay)
                .OnCancel(callback => CancelRebind(callback))
                .OnComplete(callback => FinishRebind(callback))
                .Start();
        }
    }

    private void CancelRebind(RebindingOperation callback)
    {
        callback.action.ApplyBindingOverride(GameManager.InputDetection.controlSchemeIndex, originalAction.bindings[GameManager.InputDetection.controlSchemeIndex]);
    }

    private void FinishRebind(RebindingOperation callback)
    {
        //globalMenuManager.DisablePopUpMenu();

        if (GameManager.InputMapping.Map.ContainsKey(callback.action.bindings[GameManager.InputDetection.controlSchemeIndex].effectivePath))
        {
            AudioManager.Instance.uiSfxSounds.Play("ApplyRebind");

            InputAction result = CheckIfAsigned(callback.action);
            if (result != null)
            {
                result.ApplyBindingOverride(GameManager.InputDetection.controlSchemeIndex, "");
            }
        }
        else callback.Cancel();

        GameManager.InputMapping.SaveUserRebinds(GameManager.PlayerInput);
        callback.Dispose();
        GameManager.InputDetection.controlsChangedEvent.Invoke();
    }

    private InputAction CheckIfAsigned(InputAction action)
    {
        InputAction result = null;
        InputBinding actualBinding = action.bindings[GameManager.InputDetection.controlSchemeIndex];

        foreach (InputBinding binding in action.actionMap.bindings)
        {

            if (binding.action == actualBinding.action)
            {
                continue;
            }

            if (binding.effectivePath == actualBinding.effectivePath)
            {
                result = GameManager.PlayerInput.actions.FindAction(binding.action);
                break;
            }
        }

        return result;
    }
}
