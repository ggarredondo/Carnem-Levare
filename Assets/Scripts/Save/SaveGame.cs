using UnityEngine;

[CreateAssetMenu(fileName = "SaveConfig", menuName = "Scriptable Objects/Configuration/SaveGame")]
public class SaveGame : ScriptableObject
{
    public int numGameSlots;

    public GameSlot defaultGame;
}
