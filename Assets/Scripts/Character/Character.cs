using UnityEngine;
using System;

public abstract class Character : MonoBehaviour
{
    protected Controller controller;
    protected CharacterMovement movement;
    protected CharacterStats stats;

    protected IState currentState;
    protected WalkingState walkingState;
    protected BlockingState blockingState;

    private Action transitionToWalking, transitionToBlocking;

    // Initializers
    protected virtual void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        stats = GetComponent<CharacterStats>();

        walkingState = new WalkingState();
        blockingState = new BlockingState();

        transitionToWalking = () => ChangeState(walkingState);
        transitionToBlocking = () => ChangeState(blockingState);

        ChangeState(walkingState);
    }
    protected virtual void Start() {}

    // State handler
    protected virtual void Update()
    {
        currentState.Update(this);
    }
    protected virtual void FixedUpdate()
    {
        currentState.FixedUpdate(this);
    }
    protected void ChangeState(in IState newState)
    {
        if (currentState != null) currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    // Public
    public ref readonly Controller Controller { get => ref controller; }
    public ref readonly CharacterMovement Movement { get => ref movement; }

    public ref readonly WalkingState WalkingState { get => ref walkingState; }
    public ref readonly BlockingState BlockingState { get => ref blockingState; }

    public ref readonly Action TransitionToWalking { get => ref transitionToWalking; }
    public ref readonly Action TransitionToBlocking { get => ref transitionToBlocking; }
}
