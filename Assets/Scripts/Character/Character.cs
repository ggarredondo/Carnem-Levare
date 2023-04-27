using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Character : MonoBehaviour
{
    protected Controller controller;
    protected CharacterMovement movement;
    protected CharacterStats stats;
    protected CharacterAnimation characAnimation;
    [SerializeField] private List<Move> moveList;

    protected IState currentState;
    protected WalkingState walkingState;
    protected BlockingState blockingState;

    // Initializers
    protected virtual void Awake()
    {
        walkingState = new WalkingState(this);
        blockingState = new BlockingState(this);

        movement = GetComponent<CharacterMovement>();
        movement.Initialize();

        stats = GetComponent<CharacterStats>();
        stats.Initialize();

        characAnimation = GetComponent<CharacterAnimation>();
        characAnimation.Initialize();

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
    public void ChangeState(in IState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // Public
    public ref readonly Controller Controller { get => ref controller; }
    public ref readonly CharacterMovement Movement { get => ref movement; }
    public ref readonly List<Move> MoveList { get => ref moveList; }

    public ref readonly WalkingState WalkingState { get => ref walkingState; }
    public ref readonly BlockingState BlockingState { get => ref blockingState; }
}
