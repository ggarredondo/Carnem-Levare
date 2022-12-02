using UnityEngine;

public class VisualSaver : MonoBehaviour
{
    [Header("Visual Mixer")]
    public static bool fullscreen;
    public static int vsync;
    public static string resolution;
    public static int quality;

    public static void ApplyChanges()
    {
        QualitySettings.vSyncCount = vsync;

        string[] resolutionArray = resolution.Split('x');
        Screen.SetResolution(int.Parse(resolutionArray[0]), int.Parse(resolutionArray[1]), fullscreen);

        QualitySettings.SetQualityLevel(quality, false);

        //PERMANENT CHANGES
        SaveManager.Instance.activeSave.vsync = vsync;
        SaveManager.Instance.activeSave.fullscreen = fullscreen;
        SaveManager.Instance.activeSave.resolution = resolution;
        SaveManager.Instance.activeSave.quality = quality;
    }

    public static void LoadChanges()
    {
        vsync = SaveManager.Instance.activeSave.vsync;
        fullscreen = SaveManager.Instance.activeSave.fullscreen;
        resolution = SaveManager.Instance.activeSave.resolution;
        quality = SaveManager.Instance.activeSave.quality;
    }
}
