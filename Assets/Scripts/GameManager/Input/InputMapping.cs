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
    }

    public string ObtainAllowedMapping(string path)
    {
        if (AllowedMap.ContainsKey(path))
            return AllowedMap[path];
        else
        {
            Debug.LogWarning(path + ": key was not found in the dictionary");
            return "";
        }
    }

    public string ObtainMapping(string path)
    {
        if (Map.ContainsKey(path))
            return Map[path];
        else
        {
            Debug.LogWarning(path + ": key was not found in the dictionary");
            return "";
        }
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
