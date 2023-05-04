using UnityEngine;

[CreateAssetMenu(fileName = "SaveConfig", menuName = "Scriptable Objects/Configuration/Save")]
public class SaveConfiguration : ScriptableObject
{
    public int numGameSlots;

    public OptionsSlot defaultOptions;
    public GameSlot defaultGame;
}
