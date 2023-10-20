using System.Reflection;
using UnityEngine;

public class VisualsMenu : AbstractMenu
{

    [Header("UI Elements")]
    [SerializeField] private MyToggle fullscreen;
    [SerializeField] private MyToggle vsync;
    [SerializeField] private MyToggle castShadows;
    [SerializeField] private MyToggle softShadows;
    [SerializeField] private MyToggle anisotropic;
    [SerializeField] private MyToggle softParticles;
    [SerializeField] private MyDropdown resolution;
    [SerializeField] private MyDropdown quality;
    [SerializeField] private MyDropdown antiAliasing;
    [SerializeField] private MyDropdown shadowDistance;
    [SerializeField] private MyDropdown shadowResolution;
    [SerializeField] private MyDropdown shadowCascade;
    [SerializeField] private MyDropdown textureResolution;

    [Header("Visual Config")]
    [SerializeField] private VisualOptionsApplier applier;

    protected override void Configure()
    {
        applier.Initialize();

        fullscreen.Value = DataSaver.Options.fullscreen;
        vsync.Value = DataSaver.Options.vSync;
        resolution.SetValue(DataSaver.Options.resolution);
        quality.Value = DataSaver.Options.quality;
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
        Dropdown(ref DataSaver.Options.resolution, resolution.GetText(value));
    }

    public void ChangeQuality(int value)
    {
        Dropdown(ref DataSaver.Options.quality, value);
        UIUpdateQualitySettings(value);
    }

    private void CustomDataUpdate()
    {
        applier.GetCustomQuality().antiAliasing = antiAliasing.Value;
        applier.GetCustomQuality().shadowDistance = shadowDistance.Value;
        applier.GetCustomQuality().shadowResolution = shadowResolution.Value;
        applier.GetCustomQuality().shadowCascade = shadowCascade.Value;
        applier.GetCustomQuality().textureResolution = textureResolution.Value;
        applier.GetCustomQuality().anisotropic = anisotropic.Value;
        applier.GetCustomQuality().softParticles = softParticles.Value;
        applier.GetCustomQuality().castShadows = castShadows.Value;
        applier.GetCustomQuality().softShadows = softShadows.Value;
    }

    private void UIUpdateQualitySettings(int value)
    {
        elements.ForEach(element => element.RemoveListener());

        antiAliasing.Value = applier.GetQuality(value).antiAliasing;
        shadowDistance.Value = applier.GetQuality(value).shadowDistance;
        shadowResolution.Value = applier.GetQuality(value).shadowResolution;
        shadowCascade.Value = applier.GetQuality(value).shadowCascade;
        textureResolution.Value = applier.GetQuality(value).textureResolution;
        anisotropic.Value = applier.GetQuality(value).anisotropic;
        softParticles.Value = applier.GetQuality(value).softParticles;
        castShadows.Value = applier.GetQuality(value).castShadows;
        castShadows.Event.Invoke(applier.GetQuality(value).castShadows);
        softShadows.Value = applier.GetQuality(value).softShadows;

        elements.ForEach(element => element.AddListener());
    }

    private void ChangeToCustom()
    {
        if (quality.Value != applier.CustomIndex)
        {
            quality.RemoveListener();
            quality.Value = applier.CustomIndex;
            Dropdown(ref DataSaver.Options.quality, applier.CustomIndex);
            CustomDataUpdate();
            quality.AddListener();
        }
        else GameManager.Audio.Play("PressButton");
    }

    public void ChangeAntiAliasing(int value)
    {
        applier.GetCustomQuality().antiAliasing = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeShadowDistance(int value)
    {
        applier.GetCustomQuality().shadowDistance = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeShadowResolution(int value)
    {
        applier.GetCustomQuality().shadowResolution = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeShadowCascade(int value)
    {
        applier.GetCustomQuality().shadowCascade = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeTextureResolution(int value)
    {
        applier.GetCustomQuality().textureResolution = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeAnisotropic(bool value)
    {
        applier.GetCustomQuality().anisotropic = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeSoftParticles(bool value)
    {
        applier.GetCustomQuality().softParticles = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeCastShadows(bool value)
    {
        applier.GetCustomQuality().castShadows = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }

    public void ChangeSoftShadows(bool value)
    {
        applier.GetCustomQuality().softShadows = value;
        ChangeToCustom();
        applier.ApplyChanges();
    }
}
