using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIController : Controller
{
    private RNG reactionRNG;

    [SerializeField] private bool enableDebug;
    [SerializeField] [ConditionalField("enableDebug")] [Range(-1f, 1f)] private float horizontal, vertical;
    [SerializeField] [ConditionalField("enableDebug")] private bool block = false, lateBlock = false;
    [SerializeField] [ConditionalField("enableDebug")] private bool move0, move1, move2, move3;
 
    [Header("Object Parameters")]
    [SerializeField] private AIStateMachine AIBehaviour;
    private GameKnowledge gameKnowledge;
    [SerializeField] private List<MoveSequence> moveSequences;

    [Header("Numeric Parameters")]
    [SerializeField] private int minReactionTimeMs;
    [SerializeField] private int maxReactionTimeMs;

    public override void Initialize()
    {
        base.Initialize();
        reactionRNG = new RNG(GameManager.RANDOM_SEED);
    }
    public void Reference(in CharacterStats agentStats, in CharacterStats opponentStats,
        in CharacterStateMachine agentStateMachine, in CharacterStateMachine opponentStateMachine)
    {
        agentStateMachine.OnHurt += LateBlock;
        gameKnowledge = new GameKnowledge(agentStats, opponentStats, agentStateMachine, opponentStateMachine);
        AIBehaviour.Reference(this, gameKnowledge);
        OnValidate();
        StartCoroutine(React());
    }

    private void LateBlock(in Hitbox hitbox) {
        if (lateBlock && enableDebug) DoBlock(block = true);
    }

    private void OnValidate()
    {
        if (gameKnowledge?.AgentStateMachine != null) AIBehaviour?.Enable(!enableDebug);
        if (enableDebug) {
            movementVector.x = horizontal;
            movementVector.y = vertical;
            DoBlock(block);

            if (move0) { move0 = false; DoMove(0); }
            if (move1) { move1 = false; DoMove(1); }
            if (move2) { move2 = false; DoMove(2); }
            if (move3) { move3 = false; DoMove(3); }
        }
    }

    private IEnumerator React()
    {
        while (true) {
            yield return new WaitForSeconds((float)TimeSpan.FromMilliseconds(reactionRNG.RangeInt(minReactionTimeMs, maxReactionTimeMs)).TotalSeconds);
            AIBehaviour.CurrentState.React();
        }
    }

    public void Movement(float x, float y)
    {
        movementVector.x = x;
        movementVector.y = y;
    }
    public void PerformMove(int index) => DoMove(index);
    public void PerformBlock(bool done) => DoBlock(done);

    public ref readonly List<MoveSequence> MoveSequences => ref moveSequences;
}
