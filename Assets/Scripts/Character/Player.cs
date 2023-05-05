using UnityEngine;

public class Player : Character
{
    protected override void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("INPUT").GetComponent<InputController>();
        hitboxPrefix = "HITBOX/PLAYER/";
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        base.Awake();
    }
}
