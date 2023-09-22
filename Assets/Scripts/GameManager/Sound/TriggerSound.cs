using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    [SerializeField] private string sound;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.AudioController.Play(sound);
    }
}
