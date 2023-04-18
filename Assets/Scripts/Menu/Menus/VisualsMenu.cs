using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class VisualsMenu : MonoBehaviour
{

    [Header("Toggle")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;

    [Header("Dropdown")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;

    private void Start()
    {
        //Initilize Toggles
        fullscreenToggle.isOn = DataSaver.options.fullscreen;
        vsyncToggle.isOn = DataSaver.options.vSync == 1;

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
    }

    public void Vsync(bool changeState)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        if (changeState) vsyncToggle.isOn = !vsyncToggle.isOn;
        DataSaver.options.vSync = vsyncToggle.isOn ? 1 : 0;
        OptionsApplier.apply.Invoke();
    }

    public void FullScreen(bool changeState)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        if (changeState) fullscreenToggle.isOn = !fullscreenToggle.isOn;
        DataSaver.options.fullscreen = fullscreenToggle.isOn;
        OptionsApplier.apply.Invoke();
    }

    public void ChangeResolution()
    {
        DataSaver.options.resolution = resolutionDropdown.options[resolutionDropdown.value].text;
        OptionsApplier.apply.Invoke();
    }

    public void ChangeQuality()
    {
        DataSaver.options.quality = qualityDropdown.value;
        OptionsApplier.apply.Invoke();
    }
}
