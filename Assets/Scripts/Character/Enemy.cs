using UnityEngine;

public class Enemy : Character
{
    protected override void Awake()
    {
        controller = GetComponent<AIController>();
        hitboxPrefix = "HITBOX/ENEMY/";
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Awake();
    }
}
