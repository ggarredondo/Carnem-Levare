using UnityEngine;

public class Enemy : Character
{
    private Character player;

    protected override void Awake()
    {
        controller = GetComponent<AIController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        target = player.transform;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        ((AIController) controller).Reference(characterStats, player.CharacterStats, stateMachine, player.StateMachine);
    }
}
