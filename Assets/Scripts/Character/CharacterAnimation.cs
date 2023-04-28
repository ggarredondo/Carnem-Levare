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

        character.WalkingState.OnEnter += EnterWalkingState;
        character.WalkingState.OnExit += ExitWalkingState;

        character.BlockingState.OnEnter += EnterBlockingState;
        character.BlockingState.OnExit += ExitBlockingState;

        character.AttackingState.OnEnter += EnterAttackingState;
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

    private void EnterWalkingState() { animator.SetBool("STATE_WALKING", true); }
    private void ExitWalkingState() { animator.SetBool("STATE_WALKING", false); }

    private void EnterBlockingState() { animator.SetBool("STATE_BLOCKING", true); }
    private void ExitBlockingState() { animator.SetBool("STATE_BLOCKING", false); }

    private void EnterAttackingState(int moveIndex) { animator.SetTrigger("move" + moveIndex); }
}
