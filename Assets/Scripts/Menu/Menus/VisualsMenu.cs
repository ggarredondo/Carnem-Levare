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
        fullscreenToggle.isOn = DataSaver.options.fullscreen;
        vsyncToggle.isOn = DataSaver.options.vSync;

        //Initialize resolution Dropdown
        List<string> options = new()
        {
            Display.main.systemWidth + "x" + Display.main.systemHeight,
            "1600x900",
            "1536x864",
            "1440x900",
            "1366x768",
            "1280x720",
            "1280x1024",
            "768x1024"
        };

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = resolutionDropdown.options.FindIndex(option => option.text == DataSaver.options.resolution);

        //Initialize quality Dropdown
        List<string> quality = new()
        {
            "Low",
            "Medium",
            "High"
        };

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(quality);
        qualityDropdown.value = qualityDropdown.options.FindIndex(option => option.text == quality[DataSaver.options.quality]);

        vsyncToggle.onValueChanged.AddListener(Vsync);
        fullscreenToggle.onValueChanged.AddListener(FullScreen);
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        qualityDropdown.onValueChanged.AddListener(ChangeQuality);

        fullscreenButton.onClick.AddListener(() => fullscreenToggle.isOn = !fullscreenToggle.isOn);
        vsyncButton.onClick.AddListener(() => vsyncToggle.isOn = !vsyncToggle.isOn);
    }

    public void Vsync(bool value)
    {
        Toggle(ref DataSaver.options.vSync, value);
    }

    public void FullScreen(bool value)
    {
        Toggle(ref DataSaver.options.fullscreen, value);
    }

    public void ChangeResolution(int value)
    {
        Dropdown(ref DataSaver.options.resolution, resolutionDropdown.options[value].text);
    }

    public void ChangeQuality(int value)
    {
        Dropdown(ref DataSaver.options.quality, value);
    }
}
