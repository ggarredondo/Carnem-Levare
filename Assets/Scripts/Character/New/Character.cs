using UnityEngine;

public class Character : MonoBehaviour
{
    private static int instanceCounter;
    [SerializeField] [ReadOnlyField] private int entityID;
    public int EntityID { get => entityID; }

    private void Awake()
    {
        entityID = instanceCounter;
        instanceCounter += 1;
    }
}
