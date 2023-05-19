using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    [SerializeField] private bool debug;
    [SerializeField] [ConditionalField("debug")] [Range(-1f, 1f)] private float horizontal, vertical;
    [SerializeField] [ConditionalField("debug")] private bool block = false, lateBlock = false;
    [SerializeField] [ConditionalField("debug")] private bool move0, move1, move2, move3;

    [SerializeField] private AIStateMachine AIBehaviour;
    [SerializeField] private List<MoveSequence> sequences;

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
        AIBehaviour?.Enable(!debug);
        movementVector.x = debug ? horizontal : 0f;
        movementVector.y = debug ? vertical : 0f;
        DoBlock(debug && block);

        if (debug && move0) { move0 = false; DoMove(0); }
        if (debug && move1) { move1 = false; DoMove(1); }
        if (debug && move2) { move2 = false; DoMove(2); }
        if (debug && move3) { move3 = false; DoMove(3); }
    }

    private void Update() => AIBehaviour.CurrentState.Update();

    public void Movement(float x, float y)
    {
        movementVector.x = x;
        movementVector.y = y;
    }

    public ref readonly List<MoveSequence> Sequences => ref sequences;
}
