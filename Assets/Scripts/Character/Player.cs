using UnityEngine;

public class Player : Character
{
    protected override void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("INPUT").GetComponent<InputController>();
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        base.Awake();
    }
}
