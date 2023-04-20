using UnityEngine;

public class Player : Character
{
    protected override void Start()
    {
        movement.SetTarget(GameObject.FindGameObjectWithTag("Enemy").transform);
        base.Start();
    }
}
