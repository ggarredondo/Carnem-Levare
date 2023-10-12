using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class VisualsMenu : AbstractMenu
{

    [Header("UI Elements")]
    [SerializeField] private ToggleData fullscreen;
    [SerializeField] private ToggleData vsync;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown antiAliasingDropdown;
    [SerializeField] private TMP_Dropdown shadowDistanceDropdown;
    [SerializeField] private TMP_Dropdown textureResolutionDropdown;
    [SerializeField] private ToggleData anisotropic;
    [SerializeField] private ToggleData softParticles;

    [Header("Visual Config")]
    [SerializeField] private VisualOptionsApplier applier;
    [SerializeField] private List<string> qualityName;
    [SerializeField] private List<string> resolutions;
    [SerializeField] private List<string> antiAliasingOptions;
    [SerializeField] private List<string> shadowDistanceOptions;
    [SerializeField] private List<string> textureResolutionOptions;

    protected override void Configure()
    {
        applier.Initialize();

        fullscreen.toggle.isOn = DataSaver.Options.fullscreen;
        vsync.toggle.isOn = DataSaver.Options.vSync;

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

        textureResolutionDropdown.ClearOptions();
        textureResolutionDropdown.AddOptions(textureResolutionOptions);

        UpdateCustomQualitySettings();

        //--------------------------------------------

        vsync.toggle.onValueChanged.AddListener(Vsync);
        fullscreen.toggle.onValueChanged.AddListener(FullScreen);

        fullscreen.button.onClick.AddListener(() => fullscreen.toggle.isOn = !fullscreen.toggle.isOn);
        vsync.button.onClick.AddListener(() => vsync.toggle.isOn = !vsync.toggle.isOn);
        anisotropic.button.onClick.AddListener(() => anisotropic.toggle.isOn = !anisotropic.toggle.isOn);
        softParticles.button.onClick.AddListener(() => softParticles.toggle.isOn = !softParticles.toggle.isOn);

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
        textureResolutionDropdown.onValueChanged.RemoveAllListeners();
        anisotropic.toggle.onValueChanged.RemoveAllListeners();
        softParticles.toggle.onValueChanged.RemoveAllListeners();

        antiAliasingDropdown.value = applier.ObtainAntialiasing();
        shadowDistanceDropdown.value = applier.ObtainShadowDistance();
        textureResolutionDropdown.value = applier.ObtainTextureResolution();
        anisotropic.toggle.isOn = applier.ObtainAnisotropic();
        softParticles.toggle.isOn = applier.ObtainSoftParticles();

        antiAliasingDropdown.onValueChanged.AddListener(ChangeAntiAliasing);
        shadowDistanceDropdown.onValueChanged.AddListener(ChangeShadowDistance);
        textureResolutionDropdown.onValueChanged.AddListener(ChangeTextureResolution);
        anisotropic.toggle.onValueChanged.AddListener(ChangeAnisotropic);
        softParticles.toggle.onValueChanged.AddListener(ChangeAnisotropic);
    }

    private void ChangeToCustom()
    {
        if (qualityDropdown.value != applier.customQualityIndex)
            qualityDropdown.value = applier.customQualityIndex;
        else
            GameManager.Audio.Play("PressButton");
    }

    public void ChangeAntiAliasing(int value)
    {
        applier.ApplyAntialiasing(value);
        Dropdown(ref DataSaver.Options.antialiasing, value);
        ChangeToCustom();
    }

    public void ChangeShadowDistance(int value)
    {
        applier.ApplyShadowDistance(value);
        Dropdown(ref DataSaver.Options.shadowDistance, value);
        ChangeToCustom();
    }

    public void ChangeTextureResolution(int value)
    {
        applier.ApplyTextureResolution(value);
        Dropdown(ref DataSaver.Options.textureResolution, value);
        ChangeToCustom();
    }

    public void ChangeAnisotropic(bool value)
    {
        applier.ApplyAnisotropic(value);
        Toggle(ref DataSaver.Options.anisotropic, value);
        ChangeToCustom();
    }

    public void ChangeSoftParticles(bool value)
    {
        applier.ApplySoftParticles(value);
        Toggle(ref DataSaver.Options.softParticles, value);
        ChangeToCustom();
    }
}
