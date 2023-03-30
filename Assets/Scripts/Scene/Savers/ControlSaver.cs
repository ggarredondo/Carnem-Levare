using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class ControlSaver : MonoBehaviour
{
    public static Dictionary<string, string> mapping = new();

    public static int controlSchemeIndex;

    public delegate void StaticEventHandler();
    public static StaticEventHandler StaticEvent;

    /// <summary>
    /// Read the mapping between the ui and the control fonts
    /// </summary>
    private void Awake()
    {
        ReadMappingFile();
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("rebinds") != null)
            LoadUserRebinds(SceneManagement.Instance.PlayerInput);

        OnControlSchemeChanged(SceneManagement.Instance.PlayerInput);
    }  

    #region Public
    /// <summary>
    /// Event that trigger when the controls change between Keyboard and gamepad
    /// </summary>
    /// <param name="playerInput"></param>
    public static void OnControlSchemeChanged(PlayerInput playerInput)
    {
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard&Mouse": controlSchemeIndex = 1; break;
            case "Gamepad": controlSchemeIndex = 0; break;
        }

        StaticEvent?.Invoke();
    }

    /// <summary>
    /// Save the actual input scheme
    /// </summary>
    /// <param name="player"></param>
    public static void ApplyChanges(PlayerInput player)
    {
        SaveUserRebinds(player);
    }

    /// <summary>
    /// Obtains the letter that corresponds to the current input action
    /// </summary>
    /// <param name="buttonName">Button name (same name that the input action)</param>
    /// <returns>The letter that defines the input action in the control fonts</returns>
    public static string ObtainMapping(string buttonName)
    {
        return mapping[SceneManagement.Instance.PlayerInput.actions.FindActionMap("Main Movement").FindAction(buttonName).bindings[controlSchemeIndex].effectivePath];
    }

    #endregion

    #region Private

    /// <summary>
    /// Save the actual input mapping
    /// </summary>
    /// <param name="player"></param>
    private static void SaveUserRebinds(PlayerInput player)
    {
        var rebinds = player.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    /// <summary>
    /// Load the actual input mapping
    /// </summary>
    /// <param name="player"></param>
    private static void LoadUserRebinds(PlayerInput player)
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        player.actions.LoadBindingOverridesFromJson(rebinds);
    }

    /// <summary>
    /// Creates a Dictionary from the file Gamepad.txt that defines the relation between the controls fonts and the input action
    /// </summary>
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

    #endregion
}
