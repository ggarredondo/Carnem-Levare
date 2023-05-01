using UnityEngine;

public class Enemy : Character
{
    protected override void Awake()
    {
        controller = GetComponent<AIController>();
        hitboxPrefix = "HITBOX/ENEMY/";
        base.Awake();
        movement.SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
    }
}
