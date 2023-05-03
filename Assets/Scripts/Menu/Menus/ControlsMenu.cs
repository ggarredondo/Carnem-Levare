using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : AbstractMenu
{ 
    [Header("UI Elements")]
    [SerializeField] private Toggle rumbleToggle;
    [SerializeField] private Button rumbleButton;
    [SerializeField] private List<Button> remapButtons;
    [SerializeField] private PopUpMenu popUpMenu;

    [Header("Parameters")]
    [SerializeField] private float rebindTimeDelay = 0.25f;

    private InputRemapping inputRemapping;

    protected override void Configure()
    {
        rumbleToggle.isOn = DataSaver.options.rumble;

        GameManager.InputDetection.controlsChangedEvent.Invoke();

        inputRemapping = new();
        remapButtons.ForEach(button => button.onClick.AddListener(Remapping));
        rumbleToggle.onValueChanged.AddListener(Rumble);
        rumbleButton.onClick.AddListener(() => rumbleToggle.isOn = !rumbleToggle.isOn);
    }

    public void Rumble(bool value)
    {
        Toggle(ref DataSaver.options.rumble, value);
    }

    public void Remapping()
    {
        inputRemapping.Remapping(rebindTimeDelay, popUpMenu);
    }
}
