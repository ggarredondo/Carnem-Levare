namespace UnityEngine.Rendering.Universal
{
    [CreateAssetMenu(fileName = "MyURPAsset", menuName = "Scriptable Objects/Visuals/MyURPAsset")]
    public class MyURPAsset : ScriptableObject
    {
        [Header("Requirements")]
        public int ID;
        public VisualData data;

        [Header("Options")]
        public int antiAliasing;
        public int shadowDistance;
        public int shadowResolution;
        public int textureResolution;
        public bool anisotropic;
        public bool softParticles;

        public UniversalRenderPipelineAsset urpAsset;

        public void ApplyChanges()
        {
            urpAsset.msaaSampleCount = data.antialiasingMap[antiAliasing];
            urpAsset.shadowDistance = data.shadowDistanceMap[shadowDistance];

            QualitySettings.SetQualityLevel(ID, true);

            UnityGraphics.MainLightShadowResolution = data.shadowResolutionMap[shadowResolution].shadowResolution;
            UnityGraphics.AdditionalLightShadowResolution = data.shadowResolutionMap[shadowResolution].shadowResolution;
            UnityGraphics.AdditionalLightsShadowResolutionTierHigh = data.shadowResolutionMap[shadowResolution].tierHigh;
            UnityGraphics.AdditionalLightsShadowResolutionTierMedium = data.shadowResolutionMap[shadowResolution].tierMid;
            UnityGraphics.AdditionalLightsShadowResolutionTierLow = data.shadowResolutionMap[shadowResolution].tierLow;

            QualitySettings.masterTextureLimit = data.textureResolutionMap[textureResolution];
            QualitySettings.anisotropicFiltering = anisotropic ? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable;
            QualitySettings.softParticles = softParticles;
        }
    }
}
