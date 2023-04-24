using UnityEngine;

public class Enemy : Character
{
    protected override void Awake()
    {
        controller = GetComponent<AIController>();
        base.Awake();
        movement.SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
    }
}
