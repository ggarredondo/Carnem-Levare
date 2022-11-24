using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class VisualsMenu : MonoBehaviour
{
    private VisualSaver visualMixer;

    [Header("Toggle")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;

    [Header("Dropdown")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;

    private void Awake()
    {
        visualMixer = GameObject.FindGameObjectWithTag("VISUAL").GetComponent<VisualSaver>();

        //Initilize Toggles
        fullscreenToggle.isOn = visualMixer.fullscreen;
        vsyncToggle.isOn = visualMixer.vsync == 1;

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
        resolutionDropdown.value = resolutionDropdown.options.FindIndex(option => option.text == visualMixer.resolution);

        //Initialize quality Dropdown
        List<string> quality = new List<string>();

        quality.Add("Low");
        quality.Add("Medium");
        quality.Add("High");

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(quality);
        qualityDropdown.value = qualityDropdown.options.FindIndex(option => option.text == quality[visualMixer.quality]);
    }

    public void Vsync()
    {
        visualMixer.vsync = vsyncToggle.isOn ? 1 : 0;

        visualMixer.ApplyChanges();
    }

    public void FullScreen()
    {
        visualMixer.fullscreen = fullscreenToggle.isOn;

        visualMixer.ApplyChanges();
    }

    public void ChangeResolution()
    {
        visualMixer.resolution = resolutionDropdown.options[resolutionDropdown.value].text;
        visualMixer.ApplyChanges();
    }

    public void ChangeQuality()
    {
        visualMixer.quality = qualityDropdown.value;
        visualMixer.ApplyChanges();
    }
}
