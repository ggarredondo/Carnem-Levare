using UnityEngine;

public class AddLayoutElements : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private GameObject prefab;

    public GameObject AddElement()
    {
        GameObject tmp = Instantiate(prefab);
        tmp.transform.SetParent(gameObject.transform, false);

        return tmp;
    }
}
