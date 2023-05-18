using UnityEngine;

public class Enemy : Character
{
    private GameObject player;

    protected override void Awake()
    {
        controller = GetComponent<AIController>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        GetComponent<AIController>().Reference(stateMachine, player.GetComponent<CharacterStateMachine>());
    }
}
