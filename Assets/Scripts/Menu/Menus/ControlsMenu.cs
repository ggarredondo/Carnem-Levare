using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{ 
    [SerializeField] private MainMenuManager globalMenuManager;

    [Header("Toggle")]
    [SerializeField] private Toggle rumbleToggle;

    private InputAction action, originalAction;
    public float rebindTimeDelay = 0.25f;

    private void Awake()
    {
        rumbleToggle.isOn = ControlSaver.Instance.rumble;
    }

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

            action.PerformInteractiveRebinding(ControlSaver.Instance.controlSchemeIndex)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(rebindTimeDelay)
                .OnCancel(callback => CancelRebind(callback))
                .OnComplete(callback => FinishRebind(callback))
                .Start();
        }
    }

    public void Rumble(bool changeState)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        if (changeState) rumbleToggle.isOn = !rumbleToggle.isOn;
        ControlSaver.Instance.rumble = rumbleToggle.isOn;
        ControlSaver.Instance.ApplyChanges();
    }

    #endregion

    #region Private

    private void CancelRebind(RebindingOperation callback)
    {
        callback.action.ApplyBindingOverride(ControlSaver.Instance.controlSchemeIndex, originalAction.bindings[ControlSaver.Instance.controlSchemeIndex]);
    }

    private void FinishRebind(RebindingOperation callback)
    {
        globalMenuManager.DisablePopUpMenu();

        if (ControlSaver.Instance.mapping.ContainsKey(callback.action.bindings[ControlSaver.Instance.controlSchemeIndex].effectivePath))
        {
            AudioManager.Instance.uiSfxSounds.Play("ApplyRebind");

            InputAction result = CheckIfAsigned(callback.action);
            if (result != null)
            {
                result.ApplyBindingOverride(ControlSaver.Instance.controlSchemeIndex, "");
            }
        }
        else callback.Cancel();

        ControlSaver.Instance.ApplyInputScheme(SceneManagement.Instance.PlayerInput);
        callback.Dispose();
        LoadRemapping();
    }

    private void LoadRemapping()
    {
        ControlSaver.Instance.StaticEvent.Invoke();
    }

    private InputAction CheckIfAsigned(InputAction action)
    {
        InputAction result = null;
        InputBinding actualBinding = action.bindings[ControlSaver.Instance.controlSchemeIndex];

        foreach (InputBinding binding in action.actionMap.bindings) {

            if(binding.action == actualBinding.action)
            {
                continue;
            }

            if (binding.effectivePath == actualBinding.effectivePath)
            {
                result = SceneManagement.Instance.PlayerInput.actions.FindActionMap("Main Movement").FindAction(binding.action);
                break;
            }
        }

        return result;
    }

    #endregion
}
