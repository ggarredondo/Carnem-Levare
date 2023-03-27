using UnityEngine;

public class VisualSaver : MonoBehaviour
{
    public static VisualSaver Instance { get; private set; }

    [Header("Visual Mixer")]
    public bool fullscreen;
    public int vsync;
    public string resolution;
    public int quality;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #region Public

    /// <summary>
    /// Save and apply the changes made in the visual menu
    /// </summary>
    public void ApplyChanges()
    {
        ApplyUI();

        //PERMANENT CHANGES
        SaveManager.Instance.activeSave.vsync = vsync;
        SaveManager.Instance.activeSave.fullscreen = fullscreen;
        SaveManager.Instance.activeSave.resolution = resolution;
        SaveManager.Instance.activeSave.quality = quality;
    }

    /// <summary>
    /// Apply the changes to the unity visual options
    /// </summary>
    public void ApplyUI()
    {
        QualitySettings.vSyncCount = vsync;

        string[] resolutionArray = resolution.Split('x');
        Screen.SetResolution(int.Parse(resolutionArray[0]), int.Parse(resolutionArray[1]), fullscreen);

        QualitySettings.SetQualityLevel(quality, false);
    }

    /// <summary>
    /// Load changes from the SaveManager to obtain the initial values
    /// </summary>
    public void LoadChanges()
    {
        vsync = SaveManager.Instance.activeSave.vsync;
        fullscreen = SaveManager.Instance.activeSave.fullscreen;
        resolution = SaveManager.Instance.activeSave.resolution;
        quality = SaveManager.Instance.activeSave.quality;

        ApplyUI();
    }

    #endregion
}
