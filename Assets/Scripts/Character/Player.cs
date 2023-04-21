using UnityEngine;

public class Player : Character
{
    protected override void Start()
    {
        movement.SetTarget(GameObject.FindGameObjectWithTag("Enemy").transform);
        controller = GameObject.FindGameObjectWithTag("INPUT").GetComponent<InputController>();
        base.Start();
    }
}
