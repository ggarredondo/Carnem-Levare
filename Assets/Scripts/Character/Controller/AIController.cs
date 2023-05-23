using System.Collections;
using UnityEngine;
using System;

public class AIController : Controller
{
    private RNG rng;

    [SerializeField] private bool enableDebug;
    [SerializeField] [ConditionalField("enableDebug")] [Range(-1f, 1f)] private float horizontal, vertical;
    [SerializeField] [ConditionalField("enableDebug")] private bool block = false, lateBlock = false;
    [SerializeField] [ConditionalField("enableDebug")] private bool move0, move1, move2, move3;
 
    [Header("Parameters")]
    [SerializeField] private AIStateMachine AIBehaviour;
    private GameKnowledge gameKnowledge;

    [SerializeField] private int minReactionTimeMs, maxReactionTimeMs;
    [SerializeField] private float spacingError;
    [SerializeField] private double timingError;

    public override void Initialize()
    {
        base.Initialize();
        rng = new RNG(Guid.NewGuid().GetHashCode());
        OnHurt += LateBlock;
    }

    public void Reference(in CharacterStats agentStats, in CharacterStats opponentStats,
        in CharacterStateMachine agentStateMachine, in CharacterStateMachine opponentStateMachine)
    {
        gameKnowledge = new GameKnowledge(agentStats, opponentStats, agentStateMachine, opponentStateMachine);
        AIBehaviour.Reference(this, gameKnowledge);
        StartCoroutine(React());
    }

    private void LateBlock(in Hitbox hitbox) {
        if (lateBlock) DoBlock(block = true);
    }

    private void OnValidate()
    {
        AIBehaviour?.Enable(!enableDebug);
        movementVector.x = enableDebug ? horizontal : 0f;
        movementVector.y = enableDebug ? vertical : 0f;
        DoBlock(enableDebug && block);

        if (enableDebug && move0) { move0 = false; DoMove(0); }
        if (enableDebug && move1) { move1 = false; DoMove(1); }
        if (enableDebug && move2) { move2 = false; DoMove(2); }
        if (enableDebug && move3) { move3 = false; DoMove(3); }
    }

    private void Update() => AIBehaviour.CurrentState.Update();
    private IEnumerator React()
    {
        while (true) {
            yield return new WaitForSeconds((float)TimeSpan.FromMilliseconds(rng.RangeInt(minReactionTimeMs, maxReactionTimeMs)).TotalSeconds);
            gameKnowledge.UpdateImperfectKnowledge(rng.RangeFloat(-spacingError, spacingError));
        }
    }

    public void Movement(float x, float y)
    {
        movementVector.x = x;
        movementVector.y = y;
    }
}
