using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VisualOptionsApplier : MonoBehaviour
{
    [SerializeField] private List<UniversalRenderPipelineAsset> urpQuality;
    public int customQualityIndex;

    public void Initialize()
    {
        urpQuality[customQualityIndex].msaaSampleCount = urpQuality[DataSaver.Options.antialiasing].msaaSampleCount;
        urpQuality[customQualityIndex].shadowDistance = urpQuality[DataSaver.Options.shadowDistance].shadowDistance;
        QualitySettings.masterTextureLimit = 3 - DataSaver.Options.textureResolution;
        QualitySettings.anisotropicFiltering = DataSaver.Options.anisotropic ? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable;
        QualitySettings.softParticles = DataSaver.Options.softParticles;
    }

    public void ApplyAntialiasing(int value)
    {
        urpQuality[customQualityIndex].msaaSampleCount = urpQuality[value].msaaSampleCount;
    }

    public int ObtainAntialiasing()
    {
        return (int)Mathf.Log(urpQuality[DataSaver.Options.quality].msaaSampleCount, 2);
    }

    public void ApplyShadowDistance(int value)
    {
        urpQuality[customQualityIndex].shadowDistance = urpQuality[value].shadowDistance;
    }

    public int ObtainShadowDistance()
    {
        return (int)(urpQuality[DataSaver.Options.quality].shadowDistance / 25) - 1;
    }

    public void ApplyTextureResolution(int value)
    {
        QualitySettings.masterTextureLimit = 3 - value;
    }

    public int ObtainTextureResolution()
    {
        return 3 - QualitySettings.masterTextureLimit;
    }

    public void ApplyAnisotropic(bool value)
    {
        QualitySettings.anisotropicFiltering = value ? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable;
    }

    public bool ObtainAnisotropic()
    {
        return QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable;
    }

    public void ApplySoftParticles(bool value)
    {
        QualitySettings.softParticles = value;
    }

    public bool ObtainSoftParticles()
    {
        return QualitySettings.softParticles;
    }
}
