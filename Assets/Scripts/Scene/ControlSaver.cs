using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class ControlSaver : MonoBehaviour
{
    [SerializeField] PlayerInput firstPlayer;
    public static Dictionary<string, string> mapping = new();
    public static Dictionary<string, string> uiMapping = new();

    private void Awake()
    {
        if(PlayerPrefs.GetString("rebinds") != null)
            LoadUserRebinds(firstPlayer);

        ReadMappingFile();
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

        myFilePath = Application.streamingAssetsPath + "/Mapping/UI.txt";
        fileLines = File.ReadAllLines(myFilePath);

        foreach (string line in fileLines)
        {
            string[] actionMap = line.Split(':');
            uiMapping.Add(actionMap[0], actionMap[1]);
        }
    }
}
