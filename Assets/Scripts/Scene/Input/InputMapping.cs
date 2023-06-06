using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class InputMapping
{
    public Dictionary<string, string> Map { get; private set; }
    public Dictionary<string, string> AllowedMap { get; private set; }

    public InputMapping()
    {
        Map = ReadMappingFile("/Mapping/InputFonts.txt");
        AllowedMap = ReadMappingFile("/Mapping/AllowedInputs.txt");

        if (DataSaver.options.rebinds != "")
            LoadUserRebinds(GameManager.PlayerInput);
    }

    public string ObtainAllowedMapping(in string buttonName)
    {
        string path = GameManager.PlayerInput.actions.FindAction(buttonName).bindings[GameManager.InputDetection.controlSchemeIndex].effectivePath;
        if (AllowedMap.ContainsKey(path))
            return AllowedMap[path];
        else
        {
            Debug.LogWarning(path + ": key was not found in the dictionary");
            return "";
        }
    }

    public string ObtainMapping(in string buttonName)
    {
        string path = GameManager.PlayerInput.actions.FindAction(buttonName).bindings[GameManager.InputDetection.controlSchemeIndex].effectivePath;
        if (Map.ContainsKey(path))
            return Map[path];
        else
        {
            Debug.LogWarning(path + ": key was not found in the dictionary");
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

    private Dictionary<string, string> ReadMappingFile(string file)
    {
        string myFilePath = Application.streamingAssetsPath + file;
        string[] fileLines = File.ReadAllLines(myFilePath);
        Dictionary<string, string> map = new();

        foreach (string line in fileLines)
        {
            string[] actionMap = line.Split(':');
            map.Add(actionMap[0].Replace(" ", string.Empty), actionMap[1].Replace(" ", string.Empty));
        }

        //Empty mapping
        map.Add("", "-");

        return map;
    }

}
