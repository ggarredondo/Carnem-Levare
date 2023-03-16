using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private Foot foot;

    private void OnTriggerEnter(Collider other)
    {
        SoundEvents.Instance.Walking((int) foot);
    }

    enum Foot
    {
        LEFT,
        RIGHT
    }
}
