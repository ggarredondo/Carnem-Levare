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
    private Dictionary<GameObject, GameObject> producerParticlesTable;

    private void Awake()
    {
        producerTable = new();
        producerParticlesTable = new();

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

    private GameObject InstantiateParticles(in GameObject parent, in GameObject particles)
    {
        if (producerParticlesTable.ContainsKey(parent))
        {
            Destroy(producerParticlesTable[parent]);
            producerParticlesTable.Remove(parent);
        }

        GameObject result = Instantiate(particles, parent.transform);

        producerParticlesTable.Add(parent, result);

        return result;
    }
}

[System.Serializable] 
public struct Particles 
{ 
    public string ID;
    public GameObject prefab; 
}
