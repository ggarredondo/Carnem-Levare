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

    private void Awake()
    {
        //Initilize Toggles
        fullscreenToggle.isOn = VisualSaver.Instance.fullscreen;
        vsyncToggle.isOn = VisualSaver.Instance.vsync == 1;

        //Initialize resolution Dropdown
        List<string> options = new List<string>();

        options.Add(Display.main.systemWidth + "x" + Display.main.systemHeight);
        options.Add("1600x900");
        options.Add("1536x864");
        options.Add("1440x900");
        options.Add("1366x768");
        options.Add("1280x720");
        options.Add("1280x1024");
        options.Add("768x1024");

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = resolutionDropdown.options.FindIndex(option => option.text == VisualSaver.Instance.resolution);

        //Initialize quality Dropdown
        List<string> quality = new List<string>();

        quality.Add("Low");
        quality.Add("Medium");
        quality.Add("High");

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(quality);
        qualityDropdown.value = qualityDropdown.options.FindIndex(option => option.text == quality[VisualSaver.Instance.quality]);
    }

    public void Vsync(bool changeState)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        if (changeState) vsyncToggle.isOn = !vsyncToggle.isOn;
        VisualSaver.Instance.vsync = vsyncToggle.isOn ? 1 : 0;
        VisualSaver.Instance.ApplyChanges();
    }

    public void FullScreen(bool changeState)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        if (changeState) fullscreenToggle.isOn = !fullscreenToggle.isOn;
        VisualSaver.Instance.fullscreen = fullscreenToggle.isOn;
        VisualSaver.Instance.ApplyChanges();
    }

    public void ChangeResolution()
    {
        VisualSaver.Instance.resolution = resolutionDropdown.options[resolutionDropdown.value].text;
        VisualSaver.Instance.ApplyChanges();
    }

    public void ChangeQuality()
    {
        VisualSaver.Instance.quality = qualityDropdown.value;
        VisualSaver.Instance.ApplyChanges();
    }
}
