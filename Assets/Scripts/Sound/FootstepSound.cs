using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private string sound;

    private void OnTriggerEnter(Collider other)
    {
        AudioController.Instance.Walking(sound);
    }
}
