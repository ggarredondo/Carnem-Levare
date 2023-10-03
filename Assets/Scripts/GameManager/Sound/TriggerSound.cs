using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    [SerializeField] private string sound;
    [SerializeField] private ParticlesController particleController;
    [SerializeField] private Particles particles;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Audio.Play(sound);
        particleController.Play(particles.ID, particles.prefab);
    }
}
