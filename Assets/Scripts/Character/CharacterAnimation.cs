using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Character character;
    private Animator animator;
    [SerializeField] private float animatorSpeed = 1f;

    public void Initialize()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();

        character.Movement.OnMoveCharacter += MoveAnimation;

        character.WalkingState.OnEnter += () => EnterState("STATE_WALKING");
        character.WalkingState.OnExit += () => ExitState("STATE_WALKING");

        character.BlockingState.OnEnter += () => EnterState("STATE_BLOCKING");
        character.BlockingState.OnExit += () => ExitState("STATE_BLOCKING");
    }

    private void OnValidate()
    {
        if (animator != null) animator.speed = animatorSpeed;
    }

    private void MoveAnimation(in Vector2 direction)
    {
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }

    private void EnterState(string stateName) { animator.SetBool(stateName, true); }
    private void ExitState(string stateName) { animator.SetBool(stateName, false); }
}
