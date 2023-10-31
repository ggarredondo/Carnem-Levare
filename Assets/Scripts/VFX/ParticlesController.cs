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
    [SerializeField] private bool disableParticles;

    private Dictionary<string, GameObject> producerTable;
    private Dictionary<string, GameObject> instantiateParticlesTable;

    private void Awake()
    {
        producerTable = new();
        instantiateParticlesTable = new();

        foreach(Producer producer in producers)
        {
            producerTable.Add(producer.ID, producer.producerObject);
        }
    }

    public void Play(string ID, in GameObject particles)
    {
        if (!disableParticles && ID != "" && particles != null)
        {
            GameObject parent = producerTable[ID];
            ParticleSystem currentParticles = InstantiateParticles(parent, particles).GetComponent<ParticleSystem>();
            currentParticles.Play();
        }
    }

    private GameObject InstantiateParticles(in GameObject parent, in GameObject particles)
    {
        if (!instantiateParticlesTable.ContainsKey(particles.name))
            instantiateParticlesTable.Add(particles.name, Instantiate(particles, parent.transform));

        return instantiateParticlesTable[particles.name];
    }
}

[System.Serializable] 
public struct Particles 
{ 
    public string ID;
    public GameObject prefab; 
}
