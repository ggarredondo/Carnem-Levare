using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private List<Move> enemyDrops;

    protected override void Start()
    {
        opponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        ((AIController) controller).Reference(characterStats, opponent.CharacterStats, stateMachine,  opponent.StateMachine);
        base.Start();
    }

    public ref readonly List<Move> EnemyDrops => ref enemyDrops;
}
