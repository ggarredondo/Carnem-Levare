using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;

    private void Update()
    {
        this.transform.Rotate(rotation * Time.deltaTime);
    }
}
