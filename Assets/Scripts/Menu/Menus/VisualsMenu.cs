using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class VisualsMenu : AbstractMenu
{

    [Header("UI Elements")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Button fullscreenButton;
    [SerializeField] private Button vsyncButton;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;

    protected override void Configure()
    {
        //Initilize Toggles
        fullscreenToggle.isOn = DataSaver.Options.fullscreen;
        vsyncToggle.isOn = DataSaver.Options.vSync;

        //Display.main.systemWidth + "x" + Display.main.systemHeight

        //Initialize resolution Dropdown
        List<string> options = new()
        {
            "3840x2160",
            "2560x1440",
            "1920x1080",
            "1280x720",
            "960x540",
            "640x480"
        };

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = resolutionDropdown.options.FindIndex(option => option.text == DataSaver.Options.resolution);

        //Initialize quality Dropdown
        List<string> quality = new()
        {
            "Low",
            "Medium",
            "High"
        };

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(quality);
        qualityDropdown.value = qualityDropdown.options.FindIndex(option => option.text == quality[DataSaver.Options.quality]);

        vsyncToggle.onValueChanged.AddListener(Vsync);
        fullscreenToggle.onValueChanged.AddListener(FullScreen);
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        qualityDropdown.onValueChanged.AddListener(ChangeQuality);

        fullscreenButton.onClick.AddListener(() => fullscreenToggle.isOn = !fullscreenToggle.isOn);
        vsyncButton.onClick.AddListener(() => vsyncToggle.isOn = !vsyncToggle.isOn);
    }

    public void Vsync(bool value)
    {
        Toggle(ref DataSaver.Options.vSync, value);
    }

    public void FullScreen(bool value)
    {
        Toggle(ref DataSaver.Options.fullscreen, value);
    }

    public void ChangeResolution(int value)
    {
        Dropdown(ref DataSaver.Options.resolution, resolutionDropdown.options[value].text);
    }

    public void ChangeQuality(int value)
    {
        Dropdown(ref DataSaver.Options.quality, value);
    }
}
