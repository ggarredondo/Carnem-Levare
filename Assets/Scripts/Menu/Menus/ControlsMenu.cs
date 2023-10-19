using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : AbstractMenu
{ 
    [Header("UI Elements")]
    [SerializeField] private MyToggle rumble;
    [SerializeField] private List<Button> remapButtons;
    [SerializeField] private PopUpMenu popUpMenu;

    [Header("Parameters")]
    [SerializeField] private float rebindTimeDelay = 0.25f;

    private InputRemapping inputRemapping;

    protected override void Configure()
    {
        rumble.Value = DataSaver.Options.rumble;
        inputRemapping = new();
        remapButtons.ForEach(button => button.onClick.AddListener(delegate { Remapping(button.name); } ));
    }

    public void Rumble(bool value)
    {
        Toggle(ref DataSaver.Options.rumble, value);
    }

    public void Remapping(string name)
    {
        inputRemapping.Remapping(rebindTimeDelay, popUpMenu, name);
    }
}
