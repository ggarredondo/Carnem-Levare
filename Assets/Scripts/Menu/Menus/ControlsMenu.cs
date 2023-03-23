using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class ControlsMenu : MonoBehaviour
{ 
    [SerializeField] private MainMenuManager globalMenuManager;

    private InputAction action, originalAction;
    public float rebindTimeDelay = 0.25f;

    private void Start()
    {        
        LoadRemapping();
    }

    #region Public
    public void Remapping()
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");

        action = SceneManagement.Instance.PlayerInput.actions.FindActionMap("Main Movement").FindAction(EventSystem.current.currentSelectedGameObject.name);

        if (action == null)
            Debug.Log("This action not exists");
        else
        {
            globalMenuManager.PopUpMessage("Waiting for input");

            originalAction = action.Clone();

            action.PerformInteractiveRebinding(ControlSaver.controlSchemeIndex)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(rebindTimeDelay)
                .OnCancel(callback => CancelRebind(callback))
                .OnComplete(callback => FinishRebind(callback))
                .Start();
        }
    }
    #endregion

    #region Private

    private void CancelRebind(RebindingOperation callback)
    {
        callback.action.ApplyBindingOverride(ControlSaver.controlSchemeIndex, originalAction.bindings[ControlSaver.controlSchemeIndex]);
    }

    private void FinishRebind(RebindingOperation callback)
    {
        AudioManager.Instance.uiSfxSounds.Play("ApplyRebind");

        globalMenuManager.DisablePopUpMenu();

        if (ControlSaver.mapping.ContainsKey(callback.action.bindings[ControlSaver.controlSchemeIndex].effectivePath))
        {
            if (CheckIfAsigned(callback.action) != null)
            {
                callback.Cancel();
            }
        }
        else callback.Cancel();

        ControlSaver.ApplyChanges(SceneManagement.Instance.PlayerInput);
        callback.Dispose();
        LoadRemapping();
    }

    private void LoadRemapping()
    {
        ControlSaver.StaticEvent.Invoke();
    }

    private string CheckIfAsigned(InputAction action)
    {
        string result = null;
        InputBinding actualBinding = action.bindings[ControlSaver.controlSchemeIndex];

        foreach (InputBinding binding in action.actionMap.bindings) {

            if(binding.action == actualBinding.action)
            {
                continue;
            }

            if (binding.effectivePath == actualBinding.effectivePath)
            {
                result = binding.action;
                break;
            }
        }

        return result;
    }

    #endregion
}
