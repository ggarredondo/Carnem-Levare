using UnityEngine;

public class Enemy : Character
{
    protected override void Start()
    {
        movement.SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
        controller = GetComponent<AIController>();
        base.Start();
    }
}
