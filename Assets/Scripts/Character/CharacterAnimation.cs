using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Character character;
    private Animator animator;
    [SerializeField] private float animatorSpeed = 1f;

    private void Awake()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        EnterWalkingState(); // Character enters walking state in Character.cs Awake()
    }
    private void Start()
    {
        character.Movement.OnMoveCharacter += MoveAnimation;
        character.WalkingState.OnEnter += EnterWalkingState;
        character.WalkingState.OnExit += ExitWalkingState;
        character.BlockingState.OnEnter += EnterBlockingState;
        character.BlockingState.OnExit += ExitBlockingState;
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
}
