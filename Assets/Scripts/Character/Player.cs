using UnityEngine;

public class Player : Character
{
    protected override void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("INPUT").GetComponent<InputController>();
        base.Awake();
        movement.SetTarget(GameObject.FindGameObjectWithTag("Enemy").transform);
    }
}
