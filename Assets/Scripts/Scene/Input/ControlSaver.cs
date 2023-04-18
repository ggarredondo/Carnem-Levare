using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections;

public class ControlSaver : Singleton<ControlSaver>
{
    private const float GAMEPAD_DETECTION_TIME = 0.2f;

    [System.NonSerialized] public Dictionary<string, string> mapping = new();
    [System.NonSerialized] public int controlSchemeIndex;
    [System.NonSerialized] public GameObject selected;

    public delegate void StaticEventHandler();
    public StaticEventHandler StaticEvent;

    private void Start()
    {
        selected = new GameObject();
        ReadMappingFile();

        if (DataSaver.options.rebinds != "")
            LoadUserRebinds(SceneManagement.Instance.PlayerInput);

        OnControlSchemeChanged(SceneManagement.Instance.PlayerInput);
    }  

    public void OnControlSchemeChanged(PlayerInput playerInput)
    {
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard&Mouse":
                controlSchemeIndex = 1;
                EventSystem.current.SetSelectedGameObject(null);
                Cursor.visible = true;
                break;
            case "Gamepad": 
                controlSchemeIndex = 0;
                EventSystem.current.SetSelectedGameObject(selected);
                StartCoroutine(WaitGamepadDetection(GAMEPAD_DETECTION_TIME));
                Cursor.visible = false;
                break;
        }

        StaticEvent?.Invoke();
    }

    private  IEnumerator WaitGamepadDetection(float time)
    {
        SceneManagement.Instance.UiInput.enabled = false;
        yield return new WaitForSecondsRealtime(time);
        SceneManagement.Instance.UiInput.enabled = true;
    }

    public void ApplyInputScheme(PlayerInput player)
    {
        SaveUserRebinds(player);
    }

    public string ObtainMapping(string buttonName)
    {
        if (mapping.ContainsKey(SceneManagement.Instance.PlayerInput.actions.FindActionMap("Main Movement").FindAction(buttonName).bindings[controlSchemeIndex].effectivePath))
            return mapping[SceneManagement.Instance.PlayerInput.actions.FindActionMap("Main Movement").FindAction(buttonName).bindings[controlSchemeIndex].effectivePath];
        else
        {
            Debug.LogWarning("key was not found in the dictionary");
            return "";
        }
    }

    private void SaveUserRebinds(PlayerInput player)
    {
        DataSaver.options.rebinds = player.actions.SaveBindingOverridesAsJson();
    }

    private void LoadUserRebinds(PlayerInput player)
    {
        player.actions.LoadBindingOverridesFromJson(DataSaver.options.rebinds);
    }

    private void ReadMappingFile()
    {
        string myFilePath = Application.streamingAssetsPath + "/Mapping/Gamepad.txt";
        string[] fileLines = File.ReadAllLines(myFilePath);

        foreach (string line in fileLines)
        {
            string[] actionMap = line.Split(':');
            mapping.Add(actionMap[0].Replace(" ",string.Empty) ,actionMap[1].Replace(" ", string.Empty));
        }

        //Empty mapping
        mapping.Add("", "-");
    }
}
