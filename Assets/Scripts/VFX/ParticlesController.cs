using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{

    [System.Serializable]
    private struct Producer
    {
        public string ID;
        public GameObject producerObject;
    }

    [SerializeField] private List<Producer> producers;

    private Dictionary<string, GameObject> producerTable;

    private void Awake()
    {
        foreach(Producer producer in producers)
        {
            producerTable.Add(producer.ID, producer.producerObject);
        }
    }

    public void Play(string ID, in GameObject particles)
    {
        GameObject parent = producerTable[ID];

        ParticleSystem actualParticles = InstantiateParticles(parent, particles).GetComponent<ParticleSystem>();

        actualParticles.Play();
    }

    public void Play(string ID, in GameObject particles, float duration)
    {
        GameObject parent = producerTable[ID];

        ParticleSystem actualParticles = InstantiateParticles(parent, particles).GetComponent<ParticleSystem>();

        var main = actualParticles.main;
        main.duration = duration;

        actualParticles.Play();
    }

    private GameObject InstantiateParticles(in GameObject parent, in GameObject particles)
    {
        GameObject actualParticles = parent.GetComponentInChildren<ParticleSystem>().gameObject;

        if (actualParticles != null)
            Destroy(actualParticles);

        return Instantiate(particles, parent.transform);
    }
}
