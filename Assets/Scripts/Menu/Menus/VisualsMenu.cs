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
    [SerializeField] private TMP_Dropdown shadowResolutionDropdown;
    [SerializeField] private TMP_Dropdown textureResolutionDropdown;
    [SerializeField] private ToggleData anisotropic;
    [SerializeField] private ToggleData softParticles;

    [Header("Visual Config")]
    [SerializeField] private VisualOptionsApplier applier;
    [SerializeField] private List<string> qualityName;
    [SerializeField] private List<string> resolutions;
    [SerializeField] private List<string> antiAliasingOptions;
    [SerializeField] private List<string> shadowDistanceOptions;
    [SerializeField] private List<string> shadowResolutionOptions;
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

        shadowResolutionDropdown.ClearOptions();
        shadowResolutionDropdown.AddOptions(shadowResolutionOptions);

        textureResolutionDropdown.ClearOptions();
        textureResolutionDropdown.AddOptions(textureResolutionOptions);

        UpdateCustomQualitySettings(qualityDropdown.value);

        //--------------------------------------------

        vsync.toggle.onValueChanged.AddListener(Vsync);
        fullscreen.toggle.onValueChanged.AddListener(FullScreen);

        fullscreen.button.onClick.AddListener(() => fullscreen.toggle.isOn = !fullscreen.toggle.isOn);
        vsync.button.onClick.AddListener(() => vsync.toggle.isOn = !vsync.toggle.isOn);
        anisotropic.button.onClick.AddListener(() => ChangeAnisotropic(!applier.CustomAnisotropic));
        softParticles.button.onClick.AddListener(() => ChangeSoftParticles(!applier.CustomSoftParticles));

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        qualityDropdown.onValueChanged.AddListener(ChangeQuality);
        ChangeQuality(DataSaver.Options.quality);
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
        UpdateCustomQualitySettings(value);
    }

    private void UpdateCustomQualitySettings(int value)
    {
        antiAliasingDropdown.onValueChanged.RemoveAllListeners();
        shadowDistanceDropdown.onValueChanged.RemoveAllListeners();
        shadowResolutionDropdown.onValueChanged.RemoveAllListeners();
        textureResolutionDropdown.onValueChanged.RemoveAllListeners();
        anisotropic.toggle.onValueChanged.RemoveAllListeners();
        softParticles.toggle.onValueChanged.RemoveAllListeners();

        if (value == applier.CustomIndex)
        {
            antiAliasingDropdown.value = applier.CustomAntiAliasing;
            shadowDistanceDropdown.value = applier.CustomShadowDistance;
            shadowResolutionDropdown.value = applier.CustomShadowResolution;
            textureResolutionDropdown.value = applier.CustomTextureResolution;
            anisotropic.toggle.isOn = applier.CustomAnisotropic;
            softParticles.toggle.isOn = applier.CustomSoftParticles;
        }
        else
        {
            antiAliasingDropdown.value = applier.GetQuality(value).antiAliasing;
            shadowDistanceDropdown.value = applier.GetQuality(value).shadowDistance;
            shadowResolutionDropdown.value = applier.GetQuality(value).shadowResolution;
            textureResolutionDropdown.value = applier.GetQuality(value).textureResolution;
            anisotropic.toggle.isOn = applier.GetQuality(value).anisotropic;
            softParticles.toggle.isOn = applier.GetQuality(value).softParticles;
        }

        antiAliasingDropdown.onValueChanged.AddListener(ChangeAntiAliasing);
        shadowDistanceDropdown.onValueChanged.AddListener(ChangeShadowDistance);
        shadowResolutionDropdown.onValueChanged.AddListener(ChangeShadowResolution);
        textureResolutionDropdown.onValueChanged.AddListener(ChangeTextureResolution);
        anisotropic.toggle.onValueChanged.AddListener(ChangeAnisotropic);
        softParticles.toggle.onValueChanged.AddListener(ChangeSoftParticles);
    }

    private void ChangeToCustom()
    {
        if (qualityDropdown.value != applier.CustomIndex)
        {
            qualityDropdown.onValueChanged.RemoveAllListeners();
            qualityDropdown.value = applier.CustomIndex;
            ChangeQuality(applier.CustomIndex);
            qualityDropdown.onValueChanged.AddListener(ChangeQuality);
        }
        else
        {
            UpdateCustomQualitySettings(applier.CustomIndex);
            GameManager.Audio.Play("PressButton");
        }
    }

    public void ChangeAntiAliasing(int value)
    {
        applier.CustomAntiAliasing = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeShadowDistance(int value)
    {
        applier.CustomShadowDistance = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeShadowResolution(int value)
    {
        applier.CustomShadowResolution = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeTextureResolution(int value)
    {
        applier.CustomTextureResolution = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeAnisotropic(bool value)
    {
        applier.CustomAnisotropic = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeSoftParticles(bool value)
    {
        applier.CustomSoftParticles = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }
}
