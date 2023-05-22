using UnityEngine;
using System.Threading.Tasks;
using System;

public class AIController : Controller
{
    [SerializeField] private bool enableDebug;
    [SerializeField] [ConditionalField("enableDebug")] [Range(-1f, 1f)] private float horizontal, vertical;
    [SerializeField] [ConditionalField("enableDebug")] private bool block = false, lateBlock = false;
    [SerializeField] [ConditionalField("enableDebug")] private bool move0, move1, move2, move3;
 
    [Header("Parameters")]
    [SerializeField] private AIStateMachine AIBehaviour;
    [SerializeField] private double reactionTimeMs;
    [SerializeField] private double reactionTimeError;
    [SerializeField] private float spacingError;
    [SerializeField] private double timingError;

    public override void Initialize()
    {
        base.Initialize();
        OnHurt += LateBlock;
    }

    public void Reference(in CharacterStats agentStats, in CharacterStats opponentStats,
        in CharacterStateMachine agentStateMachine, in CharacterStateMachine opponentStateMachine)
    {
        GameKnowledge gameKnowledge = new GameKnowledge(agentStats, opponentStats, agentStateMachine, opponentStateMachine);
        AIBehaviour.Reference(this, gameKnowledge);
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

    public void Movement(float x, float y)
    {
        movementVector.x = x;
        movementVector.y = y;
    }
}
