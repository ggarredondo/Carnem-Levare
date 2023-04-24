using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class InputMapping
{
    public Dictionary<string, string> Map { get; private set; }

    public InputMapping()
    {
        Map = new();
        ReadMappingFile();

        if (DataSaver.options.rebinds != "")
            LoadUserRebinds(GameManager.PlayerInput);
    }  

    public string ObtainMapping(in string buttonName)
    {
        string path = GameManager.PlayerInput.actions.FindAction(buttonName).bindings[GameManager.InputDetection.controlSchemeIndex].effectivePath;
        if (Map.ContainsKey(path))
            return Map[path];
        else
        {
            Debug.LogWarning("key was not found in the dictionary");
            return "";
        }
    }

    public void SaveUserRebinds(in PlayerInput player)
    {
        DataSaver.options.rebinds = player.actions.SaveBindingOverridesAsJson();
    }

    private void LoadUserRebinds(in PlayerInput player)
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
            Map.Add(actionMap[0].Replace(" ",string.Empty) ,actionMap[1].Replace(" ", string.Empty));
        }

        //Empty mapping
        Map.Add("", "-");
    }
}
