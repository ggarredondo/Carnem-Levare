using UnityEngine;

[CreateAssetMenu(fileName = "SaveConfig", menuName = "Scriptable Objects/Configuration/SaveOptions")]
public class SaveOptions : ScriptableObject
{
    public OptionsSlot defaultOptions;

    public void Configure()
    {
        defaultOptions.resolution = Display.main.systemWidth + "x" + Display.main.systemHeight;
    }
}
