using UnityEngine;

public class VisualSavior : MonoBehaviour
{
    [Header("Visual Mixer")]
    public bool fullscreen;
    public int vsync;
    public string resolution;
    public int quality;

    public void ApplyChanges()
    {
        QualitySettings.vSyncCount = vsync;

        string[] resolutionArray = resolution.Split('x');
        Screen.SetResolution(int.Parse(resolutionArray[0]), int.Parse(resolutionArray[1]), fullscreen);

        QualitySettings.SetQualityLevel(quality, false);
    }
}
