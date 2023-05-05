using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected CharacterStateMachine stateMachine;
    protected Controller controller;
    [SerializeField] protected CharacterMovement movement;
    [SerializeField] protected CharacterStats stats;
    [SerializeField] protected CharacterAnimation characterAnimation;
    protected CharacterAudio characAudio;
    protected string hitboxPrefix;
    protected Transform target;

    protected virtual void Awake()
    {
        stateMachine = GetComponent<CharacterStateMachine>();
        stats.Initialize(this, GetComponent<Rigidbody>());
        characterAnimation.Initialize(this, GetComponent<Animator>());
        movement.Initialize(transform, target, GetComponent<Rigidbody>());
        stateMachine.Initialize();

        stats.SubscribeEvents(stateMachine);
        characterAnimation.SubscribeEvents(stateMachine, movement);
        characAudio = new CharacterAudio(this);

        stateMachine.TransitionToWalking(); // Must be done last
    }
    protected virtual void Start() {}

    protected virtual void OnValidate()
    {
        characterAnimation.OnValidate();
    }

    public ref readonly CharacterStateMachine StateMachine { get => ref stateMachine; }
    public ref readonly Controller Controller { get => ref controller; }
    public ref readonly CharacterMovement Movement { get => ref movement; }
    public ref readonly CharacterStats Stats { get => ref stats; }
    public ref readonly CharacterAnimation CharacterAnimation { get => ref characterAnimation; }
    public ref readonly CharacterAudio CharacterAudio { get => ref CharacterAudio; }
    public ref readonly string HitboxPrefix { get => ref hitboxPrefix; }
}
