using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private CharacterMovement movement;
    private Animator animator;
    [SerializeField] private float animatorSpeed = 1f;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        animator.SetBool("STATE_WALKING", true);
    }
    private void Start()
    {
        movement.OnMoveCharacter += MoveAnimation;
    }

    private void OnValidate()
    {
        if (animator != null) animator.speed = animatorSpeed;
    }

    private void MoveAnimation(Vector2 direction)
    {
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }
}
