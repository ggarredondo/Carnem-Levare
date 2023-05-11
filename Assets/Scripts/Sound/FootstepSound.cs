using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private Foot foot;
    [SerializeField] private Entity entity;

    private void OnTriggerEnter(Collider other)
    {
        AudioController.Instance.Walking((int) foot, entity);
    }

    enum Foot
    {
        LEFT,
        RIGHT
    }
}
