using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class InputRemapping
{
    private InputAction action, originalAction;

    public void Remapping(float rebindTimeDelay, PopUpMenu popUp, string name)
    {
        GameManager.AudioController.Play("PressButton");

        action = GameManager.InputUtilities.FindAction(name);

        if (action == null)
            Debug.Log("This action not exists");
        else
        {
            popUp.PopUpMessage("Waiting for input");

            int controlSchemeIndex = GameManager.InputUtilities.ControlSchemeIndex;

            originalAction = action.Clone();

            action.PerformInteractiveRebinding(controlSchemeIndex)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(rebindTimeDelay)
                .OnCancel(callback => CancelRebind(callback, controlSchemeIndex))
                .OnComplete(callback => FinishRebind(callback, popUp, controlSchemeIndex))
                .Start();
        }
    }

    private void CancelRebind(RebindingOperation callback, int controlSchemeIndex)
    {
        callback.action.ApplyBindingOverride(controlSchemeIndex, originalAction.bindings[controlSchemeIndex]);
    }

    private void FinishRebind(RebindingOperation callback, PopUpMenu popUp, int controlSchemeIndex)
    {
        popUp.DisablePopUpMenu();

        if (GameManager.InputUtilities.ObtainAllowedMapping(callback.action) != "")
        {
            GameManager.AudioController.Play("ApplyRebind");

            InputAction result = CheckIfAsigned(callback.action, controlSchemeIndex);
            if (result != null && !result.bindings[controlSchemeIndex].isComposite)
            {
                result.ApplyBindingOverride(controlSchemeIndex, "");
            }
        }
        else callback.Cancel();

        GameManager.InputUtilities.SaveUserRebinds();
        callback.Dispose();
        GameManager.InputUtilities.Configure();
    }

    private InputAction CheckIfAsigned(InputAction action, int controlSchemeIndex)
    {
        InputAction result = null;
        InputBinding actualBinding = action.bindings[controlSchemeIndex];

        foreach (InputBinding binding in action.actionMap.bindings)
        {

            if (binding.action == actualBinding.action)
            {
                continue;
            }

            if (binding.effectivePath == actualBinding.effectivePath)
            {
                result = GameManager.InputUtilities.FindAction(binding.action);
                break;
            }
        }

        return result;
    }
}
