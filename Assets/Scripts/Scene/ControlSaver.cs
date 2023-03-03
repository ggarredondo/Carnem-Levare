using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class ControlSaver : MonoBehaviour
{
    private PlayerInput playerInput;
    public static Dictionary<string, string> mapping = new();

    public static int controlSchemeIndex;

    public delegate void StaticEventHandler();
    public static event StaticEventHandler StaticEvent;

    private void Awake()
    {
        ReadMappingFile();
    }

    private void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        if (PlayerPrefs.GetString("rebinds") != null)
            LoadUserRebinds(playerInput);

        OnControlSchemeChanged(playerInput);
    }

    public static void OnControlSchemeChanged(PlayerInput playerInput)
    {
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard&Mouse": controlSchemeIndex = 1; break;
            case "Gamepad": controlSchemeIndex = 0; break;
        }

        StaticEvent.Invoke();
    }

    public static void ApplyChanges(PlayerInput player)
    {
        SaveUserRebinds(player);
    }

    private static void SaveUserRebinds(PlayerInput player)
    {
        var rebinds = player.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    private static void LoadUserRebinds(PlayerInput player)
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        player.actions.LoadBindingOverridesFromJson(rebinds);
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
    }
}
