using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private SaveConfiguration saveConfig;

    private ISave saver;

    private void Awake()
    {
        saver = new DataSaver(saveConfig);
        saver.Load();
    }

    private void OnDestroy()
    {
        saver.Save();
    }
}
