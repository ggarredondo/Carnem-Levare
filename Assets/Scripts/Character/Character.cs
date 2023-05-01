using UnityEngine;
using System;

public abstract class Character : MonoBehaviour
{
    protected Controller controller;
    protected CharacterMovement movement;
    protected CharacterStats stats;
    protected CharacterAnimation characAnimation;
    protected CharacterAudio characAudio;
    protected string hitboxPrefix;

    protected IState currentState;
    protected WalkingState walkingState;
    protected BlockingState blockingState;
    protected AttackingState attackingState;
    protected HurtState hurtState;
    protected BlockedState blockedState;

    private Action transitionToMovement;
    private Action<int> transitionToAttacking;

    // Initializers
    protected virtual void Awake()
    {
        walkingState = new WalkingState(this);
        blockingState = new BlockingState(this);
        attackingState = new AttackingState(this);
        hurtState = new HurtState(this);
        blockedState = new BlockedState(this);

        transitionToMovement = () => ChangeState(controller.isBlocking ? blockingState : walkingState);
        transitionToAttacking = (int moveIndex) =>
        {
            if (moveIndex >= 0 && moveIndex < stats.MoveList.Count)
            {
                attackingState.moveIndex = moveIndex;
                ChangeState(attackingState);
            }
        };

        movement = GetComponent<CharacterMovement>();
        movement.Initialize();

        stats = GetComponent<CharacterStats>();
        stats.Initialize();

        characAnimation = GetComponent<CharacterAnimation>();
        characAnimation.Initialize();

        characAudio = new CharacterAudio(this);

        stats.MoveList.ForEach(move => move.Initialize(this));

        ChangeState(walkingState);
    }
    protected virtual void Start() {}

    // State handler
    protected virtual void Update()
    {
        currentState.Update();
    }
    protected virtual void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
    protected void ChangeState(in IState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // Public
    public ref readonly Controller Controller { get => ref controller; }
    public ref readonly CharacterMovement Movement { get => ref movement; }
    public ref readonly CharacterStats Stats { get => ref stats; }
    public ref readonly string HitboxPrefix { get => ref hitboxPrefix; }

    public ref readonly Action TransitionToMovement { get => ref transitionToMovement; }
    public ref readonly Action<int> TransitionToAttacking { get => ref transitionToAttacking; }

    public ref readonly IState CurrentState { get => ref currentState; }
    public ref readonly WalkingState WalkingState { get => ref walkingState; }
    public ref readonly BlockingState BlockingState { get => ref blockingState; }
    public ref readonly AttackingState AttackingState { get => ref attackingState; }
}
