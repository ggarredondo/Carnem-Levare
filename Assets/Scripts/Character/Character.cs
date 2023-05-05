using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Controller controller;
    protected CharacterStateMachine stateMachine;
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected CharacterMovement characterMovement;
    [SerializeField] protected CharacterAnimation characterAnimation;
    [SerializeField] protected CharacterAudio characterAudio;
    protected string hitboxPrefix;
    protected Transform target;

    protected virtual void Awake()
    {
        stateMachine = GetComponent<CharacterStateMachine>();
        characterStats.Initialize(this, GetComponent<Rigidbody>());
        characterMovement.Initialize(transform, target, GetComponent<Rigidbody>());
        characterAnimation.Initialize(GetComponent<Animator>());
        characterAudio.Initialize();

        stateMachine.Reference(controller, characterStats, characterMovement);
        characterStats.Reference(stateMachine);
        characterMovement.Reference();
        characterAudio.Reference(stateMachine, characterStats);
        characterAnimation.Reference(stateMachine, characterStats, characterMovement);

        stateMachine.TransitionToWalking(); // Must be done last
    }
    protected virtual void Start() {}

    protected virtual void OnValidate()
    {
        characterAnimation.OnValidate();
    }

    public ref readonly CharacterStateMachine StateMachine { get => ref stateMachine; }
    public ref readonly Controller Controller { get => ref controller; }
    public ref readonly CharacterMovement CharacterMovement { get => ref characterMovement; }
    public ref readonly CharacterStats CharacterStats { get => ref characterStats; }
    public ref readonly CharacterAnimation CharacterAnimation { get => ref characterAnimation; }
    public ref readonly CharacterAudio CharacterAudio { get => ref characterAudio; }
    public ref readonly string HitboxPrefix { get => ref hitboxPrefix; }
}
