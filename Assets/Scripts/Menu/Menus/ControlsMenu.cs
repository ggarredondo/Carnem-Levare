using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class ControlsMenu : MonoBehaviour
{
    private static string currentActionMap = "Main Movement";
    [SerializeField] private PlayerInput player;
    [SerializeField] private MenuManager globalMenuManager;

    private void Awake()
    {
        LoadRemapping();
    }

    public void Remapping(string action)
    {
        GameObject currentGameObject = EventSystem.current.currentSelectedGameObject;

        if (player.actions.FindActionMap(currentActionMap).FindAction(action) == null)
            Debug.Log("This action not exists");
        else
        {
            player.actions.FindActionMap(player.defaultActionMap).Disable();
            globalMenuManager.PopUpMessage("Waiting for input");
            player.actions.FindActionMap(currentActionMap).FindAction(action).PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.25f)
                .OnComplete(callback => FinishRebind(callback, currentGameObject))
                .Start();
        }
    }

    private void FinishRebind(InputActionRebindingExtensions.RebindingOperation callback, GameObject currentGameObject)
    { 
        Debug.Log(callback.action.bindings[0].overridePath);

        GameObject children = currentGameObject.transform.GetChild(0).gameObject;
        children.GetComponent<TMP_Text>().text = ControlSavior.mapping[callback.action.bindings[0].overridePath];
        callback.Dispose();

        globalMenuManager.DisablePopUpMenu();
        player.actions.FindActionMap(player.defaultActionMap).Enable();
        SaveRemapping();
    }

    public void SaveRemapping()
    {
        ControlSavior.ApplyChanges(player);
    }

    private void LoadRemapping()
    {
        RectTransform buttons = transform.GetChild(0).GetComponent<RectTransform>();

        for (int i = 0; i < buttons.childCount; i++)
        {
            string buttonText = buttons.GetChild(i).gameObject.GetComponent<TMP_Text>().text;
            string buttonAction = player.actions.FindActionMap(currentActionMap).FindAction(buttonText).bindings[0].effectivePath;
            buttons.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().text = ControlSavior.mapping[buttonAction];
        }
    }
}
