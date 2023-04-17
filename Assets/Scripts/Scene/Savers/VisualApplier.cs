using UnityEngine;

public class VisualApplier : IOptionsApplier
{
    public void ApplyChanges()
    {
        QualitySettings.vSyncCount = DataSaver.options.vSync;

        string[] resolutionArray = DataSaver.options.resolution.Split('x');
        Screen.SetResolution(int.Parse(resolutionArray[0]), int.Parse(resolutionArray[1]), DataSaver.options.fullscreen);

        QualitySettings.SetQualityLevel(DataSaver.options.quality, false);
    }
}
