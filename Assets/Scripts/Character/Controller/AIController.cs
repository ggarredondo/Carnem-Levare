using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Controller/AIController")]
public class AIController : Controller
{
    [SerializeField] private bool debug;
    [SerializeField] [ConditionalField("debug")] [Range(-1f, 1f)] private float horizontal, vertical;
    [SerializeField] [ConditionalField("debug")] private bool block = false, lateBlock = false;
    [SerializeField] [ConditionalField("debug")] private bool move0, move1, move2, move3;

    private GameKnowledge gameKnowledge;
    private AIStateMachine aiFSM;
    [SerializeField] private List<MoveSequence> sequences;

    public override void Initialize()
    {
        base.Initialize();
        OnHurt += LateBlock;
    }

    public void Reference(in CharacterStats agentStats, in CharacterStateMachine agentStateMachine, in CharacterStateMachine opponentStateMachine)
    {
        gameKnowledge = new GameKnowledge(agentStats, agentStateMachine, opponentStateMachine);
        aiFSM = new AIStateMachine(this, gameKnowledge);
    }

    private void LateBlock(in Hitbox hitbox) {
        if (lateBlock) DoBlock(block = true);
    }

    private void OnValidate()
    {
        aiFSM?.Enable(!debug);
        movementVector.x = debug ? horizontal : 0f;
        movementVector.y = debug ? vertical : 0f;
        DoBlock(debug && block);

        if (debug && move0) { move0 = false; DoMove(0); }
        if (debug && move1) { move1 = false; DoMove(1); }
        if (debug && move2) { move2 = false; DoMove(2); }
        if (debug && move3) { move3 = false; DoMove(3); }
    }

    public ref readonly List<MoveSequence> Sequences => ref sequences;
}
