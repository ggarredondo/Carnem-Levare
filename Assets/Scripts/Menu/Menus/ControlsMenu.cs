using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class ControlsMenu : MonoBehaviour
{ 

    private static readonly string currentActionMap = "Main Movement";
    private PlayerInput playerInput;

    [SerializeField] private MainMenuManager globalMenuManager;

    private string lastControlScheme, cancelMessage;
    private InputAction action, originalAction;
    public float rebindTimeDelay = 0.25f;

    private void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();
        
        LoadRemapping();
    }

    private void Update()
    {
        if (playerInput.currentControlScheme != lastControlScheme)
        {
            lastControlScheme = playerInput.currentControlScheme;
            LoadRemapping();
        }
    }

    public void Remapping()
    {
        SoundEvents.Instance.PressButton.Invoke();

        GameObject currentGameObject = EventSystem.current.currentSelectedGameObject;

        action = playerInput.actions.FindActionMap(currentActionMap).FindAction(currentGameObject.gameObject.name);

        if (action == null)
            Debug.Log("This action not exists");
        else
        {
            globalMenuManager.PopUpMessage("Waiting for input");

            originalAction = action.Clone();

            action.PerformInteractiveRebinding(ControlSaver.controlSchemeIndex)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(rebindTimeDelay)
                .OnCancel(callback => CancelRebind(callback, cancelMessage))
                .OnComplete(callback => FinishRebind(callback, currentGameObject))
                .Start();
        }
    }

    private void CancelRebind(RebindingOperation callback, string cancelMessage)
    {
        //StartCoroutine(globalMenuManager.PopUpForTime(cancelMessage));
        callback.action.ApplyBindingOverride(ControlSaver.controlSchemeIndex, originalAction.bindings[ControlSaver.controlSchemeIndex]);
    }

    private void FinishRebind(RebindingOperation callback, GameObject currentGameObject)
    {
        SoundEvents.Instance.ApplyRebind.Invoke();

        globalMenuManager.DisablePopUpMenu();

        GameObject children = currentGameObject.transform.GetChild(0).gameObject;

        if (ControlSaver.mapping.ContainsKey(callback.action.bindings[ControlSaver.controlSchemeIndex].effectivePath))
        {
            if (CheckIfAsigned(callback.action) != null)
            {
                cancelMessage = "Is equal to another assignment";
                callback.Cancel();
            }
            else
            {
                string fontPath = ControlSaver.mapping[callback.action.bindings[ControlSaver.controlSchemeIndex].effectivePath];
                children.GetComponent<TMP_Text>().text = fontPath;
            }
        }
        else
        {
            cancelMessage = "This action is not supported";
            callback.Cancel();
        }

        ControlSaver.ApplyChanges(playerInput);
        callback.Dispose();
        LoadRemapping();
    }

    private void LoadRemapping()
    {
        RectTransform buttons = transform.GetChild(0).GetComponent<RectTransform>();

        for (int i = 0; i < buttons.childCount; i++)
        {
            string buttonText = buttons.GetChild(i).gameObject.name;
            string buttonAction = playerInput.actions.FindActionMap(currentActionMap).FindAction(buttonText).bindings[ControlSaver.controlSchemeIndex].effectivePath;
            buttons.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().font = GlobalMenuVariables.Instance.inputFonts[ControlSaver.controlSchemeIndex];
            buttons.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().text = ControlSaver.mapping[buttonAction];
        }
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
}
