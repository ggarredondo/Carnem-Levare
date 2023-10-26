using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Controller controller;
    protected CharacterStateMachine stateMachine;
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected CharacterMovement characterMovement;
    [SerializeField] protected CharacterAnimation characterAnimation;
    [SerializeField] protected CharacterAudio characterAudio;
    [SerializeField] protected CharacterVisualEffects characterVisualEffects;
    protected Character opponent;

    protected virtual void Awake()
    {
        controller = GetComponent<Controller>();
        controller.Initialize();
        stateMachine = GetComponent<CharacterStateMachine>();
        stateMachine.Initialize();
        characterStats.Initialize(transform, GetComponent<Rigidbody>());
        characterMovement.Initialize(transform, GetComponent<Rigidbody>());
        characterAnimation.Initialize(GetComponent<Animator>(), GetComponent<HitStop>());
    }
    protected virtual void Start()
    {
        stateMachine.Reference(controller, characterStats, characterMovement);
        characterStats.Reference(stateMachine);
        characterMovement.Reference(opponent.transform);
        characterAudio.Reference(stateMachine, characterStats);
        characterVisualEffects.Reference(stateMachine, characterStats);
        characterAnimation.Reference(stateMachine, characterStats, characterMovement, 
            opponent.GetComponent<Animator>(), opponent.transform,
            GetComponent<Rigidbody>(), GetComponent<Collider>());
        stateMachine.TransitionToWalking(); // Must be done last
    }
    protected void OnValidate()
    {
        characterAnimation.OnValidate();
        CharacterStats.OnValidate(transform, GetComponent<Rigidbody>());
    }

    public ref readonly CharacterStateMachine StateMachine => ref stateMachine;
    public ref readonly Controller Controller => ref controller;
    public ref readonly CharacterMovement CharacterMovement => ref characterMovement;
    public ref readonly CharacterStats CharacterStats => ref characterStats;
    public ref readonly CharacterAnimation CharacterAnimation => ref characterAnimation;
    public ref readonly CharacterAudio CharacterAudio => ref characterAudio;
    public ref readonly CharacterVisualEffects CharacterVisualEffects => ref characterVisualEffects;
}
