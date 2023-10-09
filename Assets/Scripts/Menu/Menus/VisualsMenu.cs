using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering.Universal;

public class VisualsMenu : AbstractMenu
{

    [Header("UI Elements")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Button fullscreenButton;
    [SerializeField] private Button vsyncButton;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown antiAliasingDropdown;
    [SerializeField] private TMP_Dropdown shadowDistanceDropdown;

    [Header("Visual Config")]
    [SerializeField] private List<UniversalRenderPipelineAsset> urpQuality;
    [SerializeField] private int customQualityIndex;
    [SerializeField] private List<string> qualityName;
    [SerializeField] private List<string> resolutions;
    [SerializeField] private List<string> antiAliasingOptions;
    [SerializeField] private List<string> shadowDistanceOptions;

    protected override void Configure()
    {
        fullscreenToggle.isOn = DataSaver.Options.fullscreen;
        vsyncToggle.isOn = DataSaver.Options.vSync;

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutions);
        resolutionDropdown.value = resolutionDropdown.options.FindIndex(option => option.text == DataSaver.Options.resolution);

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(qualityName);
        qualityDropdown.value = qualityDropdown.options.FindIndex(option => option.text == qualityName[DataSaver.Options.quality]);

        antiAliasingDropdown.ClearOptions();
        antiAliasingDropdown.AddOptions(antiAliasingOptions);

        shadowDistanceDropdown.ClearOptions();
        shadowDistanceDropdown.AddOptions(shadowDistanceOptions);

        UpdateCustomQualitySettings();

        //--------------------------------------------

        vsyncToggle.onValueChanged.AddListener(Vsync);
        fullscreenToggle.onValueChanged.AddListener(FullScreen);
        fullscreenButton.onClick.AddListener(() => fullscreenToggle.isOn = !fullscreenToggle.isOn);
        vsyncButton.onClick.AddListener(() => vsyncToggle.isOn = !vsyncToggle.isOn);

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        qualityDropdown.onValueChanged.AddListener(ChangeQuality);
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
        UpdateCustomQualitySettings();
    }

    private void UpdateCustomQualitySettings()
    {
        antiAliasingDropdown.onValueChanged.RemoveAllListeners();
        shadowDistanceDropdown.onValueChanged.RemoveAllListeners();
        antiAliasingDropdown.value = (int)Mathf.Log(urpQuality[DataSaver.Options.quality].msaaSampleCount, 2);
        shadowDistanceDropdown.value = (int)(urpQuality[DataSaver.Options.quality].shadowDistance / 25) - 1;
        antiAliasingDropdown.onValueChanged.AddListener(ChangeAntiAliasing);
        shadowDistanceDropdown.onValueChanged.AddListener(ChangeShadowDistance);
    }

    private void CheckQualityAsset()
    {
        if(qualityDropdown.value != customQualityIndex)
            qualityDropdown.value = customQualityIndex;
        else
            GameManager.Audio.Play("PressButton");
    }

    public void ChangeAntiAliasing(int value)
    {
        urpQuality[customQualityIndex].msaaSampleCount = (int)Mathf.Pow(2, value);
        CheckQualityAsset();
    }

    public void ChangeShadowDistance(int value)
    {
        urpQuality[customQualityIndex].shadowDistance = 25 * (value+1);
        CheckQualityAsset();
    }
}
