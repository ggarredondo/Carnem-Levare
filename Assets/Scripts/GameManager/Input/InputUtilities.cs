using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputUtilities
{
    private static InputMapping inputMapping;
    private static InputDetection inputDetection;
    private static ControllerRumble controllerRumble;
    private static PlayerInput playerInput;
    private static InputSystemUIInputModule uiInput;

    public InputUtilities(InputSystemUIInputModule uiModule, InputReader inputReader)
    {
        inputReader.Initialize();
        uiInput = uiModule;
        playerInput = PlayerInput.all[0];

        inputMapping = new();
        inputDetection = new();
        controllerRumble = new();

        if (DataSaver.Options.rebinds != "")
            LoadUserRebinds();
    }

    public void Configure()
    {
        playerInput = PlayerInput.all[0];
        inputDetection.Configure();
    }

    public void Update()
    {
        inputDetection.CheckCustomControlScheme();
    }

    public void Rumble(float duration, float leftAmplitude, float rightAmplitude)
    {
        controllerRumble.Rumble(duration, leftAmplitude, rightAmplitude);
    }

    public void SwitchActionMap(string newActionMap)
    {
        playerInput.SwitchCurrentActionMap(newActionMap);
    }

    public void SetSelectedGameObject(GameObject obj)
    {
        inputDetection.selected = obj;
    }

    #region ACCESS

    public ref readonly PlayerInput PlayerInput { get => ref playerInput; }

    public ref readonly int ControlSchemeIndex { get => ref inputDetection.controlSchemeIndex; }

    public ref readonly InputDevice PreviousCustomControlScheme { get => ref inputDetection.previousCustomControlScheme; }

    public ref System.Action ControlsChangedEvent { get => ref inputDetection.controlsChangedEvent; }

    public void EnablePlayerInput(bool enable)
    {
        playerInput.enabled = enable;
    }

    public void ActivatePlayerInput(bool activate)
    {
        if (activate) playerInput.ActivateInput();
        else playerInput.DeactivateInput();
    }

    public void EnableUIModule(bool enable)
    {
        uiInput.enabled = enable;
    }

    public string CurrentControlScheme()
    {
        return playerInput.currentControlScheme;
    }

    public InputAction FindAction(string action)
    {
        return playerInput.actions.FindAction(action);
    }

    #endregion

    #region MAPPING

    private void LoadUserRebinds()
    {
        playerInput.actions.LoadBindingOverridesFromJson(DataSaver.Options.rebinds);
    }

    public void SaveUserRebinds()
    {
        DataSaver.Options.rebinds = playerInput.actions.SaveBindingOverridesAsJson();
    }

    public string ObtainAllowedMapping(in string buttonName)
    {
        string path = playerInput.actions.FindAction(buttonName).bindings[inputDetection.controlSchemeIndex].effectivePath;

        return inputMapping.ObtainAllowedMapping(path);
    }

    public string ObtainAllowedMapping(in InputAction action)
    {
        string path = action.bindings[inputDetection.controlSchemeIndex].effectivePath;

        return inputMapping.ObtainAllowedMapping(path);
    }


    public string ObtainMapping(in string buttonName)
    {
        string path = playerInput.actions.FindAction(buttonName).bindings[inputDetection.controlSchemeIndex].effectivePath;

        return inputMapping.ObtainMapping(path);
    }

    #endregion
}
