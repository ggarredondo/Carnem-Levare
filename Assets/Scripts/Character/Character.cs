using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected CharacterStateMachine stateMachine;
    protected Controller controller;
    protected CharacterMovement movement;
    protected CharacterStats stats;
    protected CharacterAnimation characAnimation;
    protected CharacterAudio characAudio;
    protected string hitboxPrefix;

    protected virtual void Awake()
    {
        stateMachine = GetComponent<CharacterStateMachine>();
        movement = GetComponent<CharacterMovement>();
        stats = GetComponent<CharacterStats>();
        characAnimation = GetComponent<CharacterAnimation>();

        stateMachine.Initialize();
        movement.Initialize();
        stats.Initialize();
        stats.MoveList.ForEach(move => move.Initialize(this));
        characAnimation.Initialize();
        characAudio = new CharacterAudio(this);

        stateMachine.TransitionToWalking(); // Must be done last
    }
    protected virtual void Start() {}

    public ref readonly CharacterStateMachine StateMachine { get => ref stateMachine; }
    public ref readonly Controller Controller { get => ref controller; }
    public ref readonly CharacterMovement Movement { get => ref movement; }
    public ref readonly CharacterStats Stats { get => ref stats; }
    public ref readonly string HitboxPrefix { get => ref hitboxPrefix; }
}
