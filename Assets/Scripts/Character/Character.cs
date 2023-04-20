using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected CharacterMovement movement;
    protected CharacterStats stats;

    protected IState currentState;
    protected WalkingState walkingState;

    // Initializers
    protected virtual void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        stats = GetComponent<CharacterStats>();

        walkingState = new WalkingState();
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
    protected void ChangeState(IState newState)
    {
        if (currentState != null) currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    // Public
    public CharacterMovement Movement { get => movement; }
}
