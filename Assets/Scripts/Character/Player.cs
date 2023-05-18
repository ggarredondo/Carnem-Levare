using UnityEngine;

public class Player : Character
{
    protected override void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        base.Awake();
    }
}
