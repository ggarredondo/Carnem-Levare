using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class ControlsMenu : MonoBehaviour
{ 

    private static readonly string currentActionMap = "Main Movement";

    [SerializeField] private PlayerInput player;
    [SerializeField] private MainMenuManager globalMenuManager;

    private string cancelMessage;
    public float rebindTimeDelay = 0.25f;

    private void Awake()
    {
        LoadRemapping();
    }

    public void Remapping()
    {
        GameObject currentGameObject = EventSystem.current.currentSelectedGameObject;

        string action = currentGameObject.gameObject.name;

        if (player.actions.FindActionMap(currentActionMap).FindAction(action) == null)
            Debug.Log("This action not exists");
        else
        {
            player.actions.FindActionMap(player.defaultActionMap).Disable();
            globalMenuManager.PopUpMessage("Waiting for input");

            player.actions.FindActionMap(currentActionMap).FindAction(action).PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(rebindTimeDelay)
                .OnCancel(callback => CancelRebind(cancelMessage))
                .OnComplete(callback => FinishRebind(callback, currentGameObject))
                .Start();
        }
    }

    private void CancelRebind(string cancelMessage)
    {
        StartCoroutine(globalMenuManager.PopUpForTime(cancelMessage));
    }

    private void FinishRebind(InputActionRebindingExtensions.RebindingOperation callback, GameObject currentGameObject)
    {
        globalMenuManager.DisablePopUpMenu();

        GameObject children = currentGameObject.transform.GetChild(0).gameObject;

        if (ControlSaver.mapping.ContainsKey(callback.action.bindings[0].effectivePath))
        {
            string fontPath = ControlSaver.mapping[callback.action.bindings[0].effectivePath];

            if (CheckIfAsigned(callback.action) != null)
            {
                print("Iguales");
            }

            children.GetComponent<TMP_Text>().text = fontPath;
            ControlSaver.ApplyChanges(player);
        }
        else
        {
            cancelMessage = "This action is not supported";
            callback.Cancel();
        }

        callback.Dispose();
        player.actions.FindActionMap(player.defaultActionMap).Enable();
    }

    private void LoadRemapping()
    {
        RectTransform buttons = transform.GetChild(0).GetComponent<RectTransform>();

        for (int i = 0; i < buttons.childCount; i++)
        {
            string buttonText = buttons.GetChild(i).gameObject.name;
            string buttonAction = player.actions.FindActionMap(currentActionMap).FindAction(buttonText).bindings[0].effectivePath;
            buttons.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().text = ControlSaver.mapping[buttonAction];
        }
    }

    private string CheckIfAsigned(InputAction action)
    {
        string result = null;
        InputBinding actualBinding = action.bindings[0];

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
