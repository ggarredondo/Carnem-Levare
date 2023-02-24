using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ControlsMenu : MonoBehaviour
{ 

    private static readonly string currentActionMap = "Main Movement";

    [SerializeField] private PlayerInput player;
    [SerializeField] private MainMenuManager globalMenuManager;
    [SerializeField] private TMP_FontAsset[] fonts;

    private string lastControlScheme;
    private int controlSchemeIndex;
    
    public float rebindTimeDelay = 0.25f;

    private void Awake()
    {
        lastControlScheme = player.defaultControlScheme;
        controlSchemeIndex = 0;
        LoadRemapping();
    }

    private void Update()
    {
        if (player.currentControlScheme != lastControlScheme)
        {
            lastControlScheme = player.currentControlScheme;
            controlSchemeIndex = (controlSchemeIndex + 1) % 2;
            LoadRemapping();
        }
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

            player.actions.FindActionMap(currentActionMap).FindAction(action).PerformInteractiveRebinding(controlSchemeIndex)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(rebindTimeDelay)
                .OnCancel(callback => CancelRebind("This action is not supported"))
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

        if (ControlSaver.mapping.ContainsKey(callback.action.bindings[controlSchemeIndex].effectivePath))
        {
            string fontPath = ControlSaver.mapping[callback.action.bindings[controlSchemeIndex].effectivePath];

            if (CheckIfAsigned(callback.action) != null)
            {
                print("Iguales");
            }

            children.GetComponent<TMP_Text>().text = fontPath;
            ControlSaver.ApplyChanges(player);
        }
        else callback.Cancel();

        callback.Dispose();
        player.actions.FindActionMap(player.defaultActionMap).Enable();
    }

    private void LoadRemapping()
    {
        RectTransform buttons = transform.GetChild(0).GetComponent<RectTransform>();

        for (int i = 0; i < buttons.childCount; i++)
        {
            string buttonText = buttons.GetChild(i).gameObject.name;
            string buttonAction = player.actions.FindActionMap(currentActionMap).FindAction(buttonText).bindings[controlSchemeIndex].effectivePath;
            buttons.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>().font = fonts[controlSchemeIndex];
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
