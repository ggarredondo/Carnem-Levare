using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    [SerializeField] private string sound;
    [SerializeField] private ParticlesController particleController;
    [SerializeField] private Particles particles;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.AudioController.Play(sound);
        particleController.Play(particles.ID, particles.prefab);
    }
}
