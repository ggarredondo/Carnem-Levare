using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    private GameObject actualEnemy;
    private GameObject defaultEnemy;
    private int actualEnemyID;
    private RNG random;

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
        defaultEnemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    public GameObject LoadEnemy()
    {
        if (DataSaver.Game.enemyPrefabNames.Count > 0)
        {
            actualEnemyID = random.RangeInt(0, DataSaver.Game.enemyPrefabNames.Count);
            string path = DataSaver.Game.enemyResourcesFolder + DataSaver.Game.enemyPrefabNames[actualEnemyID];

            actualEnemy = Instantiate((GameObject)LoadPrefabEnemyFromFile(path), defaultEnemy.transform.parent);
            actualEnemy.transform.SetPositionAndRotation(defaultEnemy.transform.position, defaultEnemy.transform.rotation);
            actualEnemy.transform.localScale = defaultEnemy.transform.localScale;

            Destroy(defaultEnemy);
        }
        else
        {
            Debug.LogWarning("No enemy prefabs in datasaver list");
            actualEnemy = defaultEnemy;
        }

        return actualEnemy;
    }

    public int GetActualEnemyId()
    {
        return actualEnemyID;
    }
}
