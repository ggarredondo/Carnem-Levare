using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    private GameObject actualEnemy;
    private int actualEnemyID;
    private RNG random;

    [SerializeField] private Transform enemyParent;

    private Object LoadPrefabEnemyFromFile(string path)
    {
        var loadedObject = Resources.Load(path);
        if (loadedObject == null)
        {
            Debug.LogWarning("...no file found - please check the configuration");
        }
        return loadedObject;
    }

    public void Initialize()
    {
        random = new RNG(GameManager.RANDOM_SEED);
    }

    public GameObject LoadEnemy()
    {
        if (DataSaver.Game.enemyPrefabNames.Count > 0)
        {
            actualEnemyID = random.RangeInt(0, DataSaver.Game.enemyPrefabNames.Count-1);
            string path = DataSaver.Game.enemyResourcesFolder + DataSaver.Game.enemyPrefabNames[actualEnemyID];

            actualEnemy = Instantiate((GameObject)LoadPrefabEnemyFromFile(path), enemyParent);
        }
        else Debug.LogWarning("No enemy prefabs in datasaver list");

        return actualEnemy;
    }

    public int GetActualEnemyId()
    {
        return actualEnemyID;
    }
}
