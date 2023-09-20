using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private string sound;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.AudioController.Play(sound);
    }
}
