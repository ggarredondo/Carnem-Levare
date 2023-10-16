using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class VisualsMenu : AbstractMenu
{

    [Header("UI Elements")]
    [SerializeField] private ToggleData fullscreen;
    [SerializeField] private ToggleData vsync;
    [SerializeField] private ToggleData castShadows;
    [SerializeField] private ToggleData softShadows;
    [SerializeField] private ToggleData anisotropic;
    [SerializeField] private ToggleData softParticles;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown antiAliasingDropdown;
    [SerializeField] private TMP_Dropdown shadowDistanceDropdown;
    [SerializeField] private TMP_Dropdown shadowResolutionDropdown;
    [SerializeField] private TMP_Dropdown shadowCascadeDropdown;
    [SerializeField] private TMP_Dropdown textureResolutionDropdown;

    [Header("Visual Config")]
    [SerializeField] private VisualOptionsApplier applier;
    [SerializeField] private List<string> qualityName;
    [SerializeField] private List<string> resolutions;
    [SerializeField] private List<string> antiAliasingOptions;
    [SerializeField] private List<string> shadowDistanceOptions;
    [SerializeField] private List<string> shadowResolutionOptions;
    [SerializeField] private List<string> shadowCascadeOptions;
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

        shadowCascadeDropdown.ClearOptions();
        shadowCascadeDropdown.AddOptions(shadowCascadeOptions);

        textureResolutionDropdown.ClearOptions();
        textureResolutionDropdown.AddOptions(textureResolutionOptions);

        UIUpdateQualitySettings(qualityDropdown.value);

        //--------------------------------------------

        vsync.toggle.onValueChanged.AddListener(Vsync);
        fullscreen.toggle.onValueChanged.AddListener(FullScreen);

        fullscreen.button.onClick.AddListener(() => fullscreen.toggle.isOn = !fullscreen.toggle.isOn);
        vsync.button.onClick.AddListener(() => vsync.toggle.isOn = !vsync.toggle.isOn);
        anisotropic.button.onClick.AddListener(() => anisotropic.toggle.isOn = !anisotropic.toggle.isOn);
        softParticles.button.onClick.AddListener(() => softParticles.toggle.isOn = !softParticles.toggle.isOn);
        castShadows.button.onClick.AddListener(() => castShadows.toggle.isOn = !castShadows.toggle.isOn);
        softShadows.button.onClick.AddListener(() => softShadows.toggle.isOn = !softShadows.toggle.isOn);

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
        UIUpdateQualitySettings(value);
    }

    private void CustomDataUpdate()
    {
        applier.CustomAntiAliasing = antiAliasingDropdown.value;
        applier.CustomShadowDistance = shadowDistanceDropdown.value;
        applier.CustomShadowResolution = shadowResolutionDropdown.value;
        applier.CustomShadowCascade = shadowCascadeDropdown.value;
        applier.CustomTextureResolution = textureResolutionDropdown.value;
        applier.CustomAnisotropic = anisotropic.toggle.isOn;
        applier.CustomSoftParticles = softParticles.toggle.isOn;
        applier.CustomCastShadows = castShadows.toggle.isOn;
        applier.CustomSoftShadows = softShadows.toggle.isOn;
    }

    private void CustomUIUpdate()
    {
        antiAliasingDropdown.value = applier.CustomAntiAliasing;
        shadowDistanceDropdown.value = applier.CustomShadowDistance;
        shadowResolutionDropdown.value = applier.CustomShadowResolution;
        shadowCascadeDropdown.value = applier.CustomShadowCascade;
        textureResolutionDropdown.value = applier.CustomTextureResolution;
        anisotropic.toggle.isOn = applier.CustomAnisotropic;
        softParticles.toggle.isOn = applier.CustomSoftParticles;
        castShadows.toggle.isOn = applier.CustomCastShadows;
        softShadows.toggle.isOn = applier.CustomSoftShadows;
    }

    private void UIUpdate(int value)
    {
        antiAliasingDropdown.value = applier.GetQuality(value).antiAliasing;
        shadowDistanceDropdown.value = applier.GetQuality(value).shadowDistance;
        shadowResolutionDropdown.value = applier.GetQuality(value).shadowResolution;
        shadowCascadeDropdown.value = applier.GetQuality(value).shadowCascade;
        textureResolutionDropdown.value = applier.GetQuality(value).textureResolution;
        anisotropic.toggle.isOn = applier.GetQuality(value).anisotropic;
        softParticles.toggle.isOn = applier.GetQuality(value).softParticles;
        castShadows.toggle.isOn = applier.GetQuality(value).castShadows;
        softShadows.toggle.isOn = applier.GetQuality(value).softShadows;
    }

    private void UIUpdateQualitySettings(int value)
    {
        antiAliasingDropdown.onValueChanged.RemoveAllListeners();
        shadowDistanceDropdown.onValueChanged.RemoveAllListeners();
        shadowResolutionDropdown.onValueChanged.RemoveAllListeners();
        shadowCascadeDropdown.onValueChanged.RemoveAllListeners();
        textureResolutionDropdown.onValueChanged.RemoveAllListeners();
        anisotropic.toggle.onValueChanged.RemoveAllListeners();
        softParticles.toggle.onValueChanged.RemoveAllListeners();
        castShadows.toggle.onValueChanged.RemoveAllListeners();
        softShadows.toggle.onValueChanged.RemoveAllListeners();

        if (value == applier.CustomIndex) CustomUIUpdate();
        else UIUpdate(value);

        CheckColor();

        antiAliasingDropdown.onValueChanged.AddListener(ChangeAntiAliasing);
        shadowDistanceDropdown.onValueChanged.AddListener(ChangeShadowDistance);
        shadowResolutionDropdown.onValueChanged.AddListener(ChangeShadowResolution);
        shadowCascadeDropdown.onValueChanged.AddListener(ChangeShadowCascade);
        textureResolutionDropdown.onValueChanged.AddListener(ChangeTextureResolution);
        anisotropic.toggle.onValueChanged.AddListener(ChangeAnisotropic);
        softParticles.toggle.onValueChanged.AddListener(ChangeSoftParticles);
        castShadows.toggle.onValueChanged.AddListener(ChangeCastShadows);
        softShadows.toggle.onValueChanged.AddListener(ChangeSoftShadows);
    }

    private void ChangeToCustom()
    {
        if (qualityDropdown.value != applier.CustomIndex)
        {
            qualityDropdown.onValueChanged.RemoveAllListeners();
            qualityDropdown.value = applier.CustomIndex;
            Dropdown(ref DataSaver.Options.quality, applier.CustomIndex);
            CustomDataUpdate();
            qualityDropdown.onValueChanged.AddListener(ChangeQuality);
        }
        else GameManager.Audio.Play("PressButton");
    }

    private void CheckColor()
    {
        if (castShadows.toggle.isOn)
        {
            ChangeColor(ref softShadows.button, new Color32(255, 255, 255, 255));
            ChangeColor(ref softShadows.toggle, new Color32(255, 255, 255, 255));
            ChangeColorTransitions(3, new Color32(255, 255, 255, 255));
            ChangeColorTransitions(5, new Color32(255, 255, 255, 255));
            ChangeColorTransitions(6, new Color32(255, 255, 255, 255));
        }
        else
        {
            ChangeColor(ref softShadows.button, new Color32(255, 255, 255, 100));
            ChangeColor(ref softShadows.toggle, new Color32(255, 255, 255, 100));
            ChangeColorTransitions(3, new Color32(255, 255, 255, 100));
            ChangeColorTransitions(5, new Color32(255, 255, 255, 100));
            ChangeColorTransitions(6, new Color32(255, 255, 255, 100));
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

    public void ChangeShadowCascade(int value)
    {
        applier.CustomShadowCascade = value;
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

    public void ChangeCastShadows(bool value)
    {
        applier.CustomCastShadows = value;
        ChangeToCustom();
        applier.ApplyChanges();
        CheckColor();
    }

    public void ChangeSoftShadows(bool value)
    {
        applier.CustomSoftShadows = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }
}
