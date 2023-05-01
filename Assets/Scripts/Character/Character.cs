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
        // Must be done first
        stateMachine = GetComponent<CharacterStateMachine>();
        stateMachine.Initialize();

        movement = GetComponent<CharacterMovement>();
        movement.Initialize();

        stats = GetComponent<CharacterStats>();
        stats.Initialize();

        characAnimation = GetComponent<CharacterAnimation>();
        characAnimation.Initialize();

        characAudio = new CharacterAudio(this);

        stats.MoveList.ForEach(move => move.Initialize(this));

        // Must be done last
        stateMachine.TransitionToWalking.Invoke();
    }
    protected virtual void Start() {}

    public ref readonly CharacterStateMachine StateMachine { get => ref stateMachine; }
    public ref readonly Controller Controller { get => ref controller; }
    public ref readonly CharacterMovement Movement { get => ref movement; }
    public ref readonly CharacterStats Stats { get => ref stats; }
    public ref readonly string HitboxPrefix { get => ref hitboxPrefix; }
}
