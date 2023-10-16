using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "VisualOptions", menuName = "Scriptable Objects/Visuals/VisualOptions")]
public class VisualOptionsApplier : ScriptableObject
{
    [SerializeField] private List<MyURPAsset> urpQuality;
    [SerializeField] private MyURPAsset urpCustom;

    public void Initialize()
    {
        foreach(MyURPAsset asset in urpQuality)
        {
            asset.ApplyChanges();
        }

        ApplyChanges();
    }

    public void ApplyChanges()
    {
        urpCustom.ApplyChanges();
    }

    public int CustomIndex { get => urpCustom.ID;}

    public int CustomAntiAliasing { get => urpCustom.antiAliasing; set => urpCustom.antiAliasing = value; }
    public bool CustomCastShadows { get => urpCustom.castShadows; set => urpCustom.castShadows = value; }
    public bool CustomSoftShadows { get => urpCustom.softShadows; set => urpCustom.softShadows = value; }
    public int CustomShadowDistance { get => urpCustom.shadowDistance; set => urpCustom.shadowDistance = value; }
    public int CustomShadowResolution { get => urpCustom.shadowResolution; set => urpCustom.shadowResolution = value; }
    public int CustomShadowCascade { get => urpCustom.shadowCascade; set => urpCustom.shadowCascade = value; }
    public int CustomTextureResolution { get => urpCustom.textureResolution; set => urpCustom.textureResolution = value; }
    public bool CustomAnisotropic { get => urpCustom.anisotropic; set => urpCustom.anisotropic = value; }
    public bool CustomSoftParticles { get => urpCustom.softParticles; set => urpCustom.softParticles = value; }

    public MyURPAsset GetQuality(int value)
    {
        return urpQuality[value];
    }
}
