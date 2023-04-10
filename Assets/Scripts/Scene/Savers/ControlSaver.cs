using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections;

public class ControlSaver : MonoBehaviour
{
    private const float GAMEPAD_DETECTION_TIME = 0.2f;

    public static ControlSaver Instance { get; private set; }

    [System.NonSerialized] public Dictionary<string, string> mapping = new();
    [System.NonSerialized] public int controlSchemeIndex;
    [System.NonSerialized] public GameObject selected;

    public delegate void StaticEventHandler();
    public StaticEventHandler StaticEvent;

    [System.NonSerialized] public bool rumble;

    /// <summary>
    /// Read the mapping between the ui and the control fonts
    /// </summary>
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            selected = new GameObject();
        }
        else
        {
            Instance = this;
            ReadMappingFile();
        }
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

    /// <summary>
    /// Save the actual input settings
    /// </summary>
    /// <param name="player"></param>
    public void ApplyChanges()
    {
        ApplyUI();

        //PERMANENT CHANGES
        SaveManager.Instance.activeSave.rumble = rumble;
    }

    public void ApplyUI()
    {
        ControllerRumble.Instance.CanRumble = rumble;
    }

    /// <summary>
    /// Load changes from the SaveManager to obtain the initial values
    /// </summary>
    public void LoadChanges()
    {
        rumble = SaveManager.Instance.activeSave.rumble;

        ApplyUI();
    }

    /// <summary>
    /// Save the actual input scheme
    /// </summary>
    /// <param name="player"></param>
    public void ApplyInputScheme(PlayerInput player)
    {
        SaveUserRebinds(player);
    }

    /// <summary>
    /// Obtains the letter that corresponds to the current input action
    /// </summary>
    /// <param name="buttonName">Button name (same name that the input action)</param>
    /// <returns>The letter that defines the input action in the control fonts</returns>
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

    #endregion

    #region Private

    /// <summary>
    /// Save the actual input mapping
    /// </summary>
    /// <param name="player"></param>
    private void SaveUserRebinds(PlayerInput player)
    {
        var rebinds = player.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    /// <summary>
    /// Load the actual input mapping
    /// </summary>
    /// <param name="player"></param>
    private void LoadUserRebinds(PlayerInput player)
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

        //Empty mapping
        mapping.Add("", "-");
    }

    #endregion
}
