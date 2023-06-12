using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private Character player;
    [SerializeField] private List<Move> enemyDrops;

    protected override void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        target = player.transform;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        ((AIController) controller).Reference(characterStats, player.CharacterStats, stateMachine, player.StateMachine);
    }

    public ref readonly List<Move> EnemyDrops => ref enemyDrops;
}
