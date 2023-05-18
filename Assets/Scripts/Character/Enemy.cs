using UnityEngine;

public class Enemy : Character
{
    protected override void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        AIController aiController = (AIController)controller;
        aiController.Reference(stateMachine, GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStateMachine>());
    }
}
