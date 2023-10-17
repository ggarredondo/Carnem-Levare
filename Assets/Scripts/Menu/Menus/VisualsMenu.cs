using System.Collections.Generic;
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


    private readonly List<ISelectable> elements = new();

    private void ObtainElementsByReflection()
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (FieldInfo field in fields)
        {
            if (typeof(ISelectable).IsAssignableFrom(field.FieldType))
            {
                elements.Add((ISelectable)field.GetValue(this));
            }
        }
    }

    protected override void Configure()
    {
        ObtainElementsByReflection();

        applier.Initialize();
        elements.ForEach(element => element.Initialize());

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
        applier.CustomAntiAliasing = antiAliasing.Value;
        applier.CustomShadowDistance = shadowDistance.Value;
        applier.CustomShadowResolution = shadowResolution.Value;
        applier.CustomShadowCascade = shadowCascade.Value;
        applier.CustomTextureResolution = textureResolution.Value;
        applier.CustomAnisotropic = anisotropic.Value;
        applier.CustomSoftParticles = softParticles.Value;
        applier.CustomCastShadows = castShadows.Value;
        applier.CustomSoftShadows = softShadows.Value;
    }

    private void CustomUIUpdate()
    {
        antiAliasing.Value = applier.CustomAntiAliasing;
        shadowDistance.Value = applier.CustomShadowDistance;
        shadowResolution.Value = applier.CustomShadowResolution;
        shadowCascade.Value = applier.CustomShadowCascade;
        textureResolution.Value = applier.CustomTextureResolution;
        anisotropic.Value = applier.CustomAnisotropic;
        softParticles.Value = applier.CustomSoftParticles;
        castShadows.Value = applier.CustomCastShadows;
        softShadows.Value = applier.CustomSoftShadows;
    }

    private void UIUpdate(int value)
    {
        antiAliasing.Value = applier.GetQuality(value).antiAliasing;
        shadowDistance.Value = applier.GetQuality(value).shadowDistance;
        shadowResolution.Value = applier.GetQuality(value).shadowResolution;
        shadowCascade.Value = applier.GetQuality(value).shadowCascade;
        textureResolution.Value = applier.GetQuality(value).textureResolution;
        anisotropic.Value = applier.GetQuality(value).anisotropic;
        softParticles.Value = applier.GetQuality(value).softParticles;
        castShadows.Value = applier.GetQuality(value).castShadows;
        softShadows.Value = applier.GetQuality(value).softShadows;
    }

    private void UIUpdateQualitySettings(int value)
    {
        elements.ForEach(element => element.RemoveListener());

        if (value == applier.CustomIndex) CustomUIUpdate();
        else UIUpdate(value);

        CheckColor();

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

    private void CheckColor()
    {
        if (castShadows.Value)
        {
            ChangeColorTransitions(3, new Color32(255, 255, 255, 255));
            ChangeColorTransitions(5, new Color32(255, 255, 255, 255));
            ChangeColorTransitions(6, new Color32(255, 255, 255, 255));
        }
        else
        {
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
