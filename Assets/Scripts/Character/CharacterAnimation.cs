using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CharacterAnimation
{
    private List<Move> moveList;
    private Animator animator;
    private HitStop hitstop;
    [SerializeField] private float animatorSpeed = 1f;

    public void Initialize(in Animator animator, in HitStop hitstop)
    {
        this.animator = animator;
        this.hitstop = hitstop; 
    }
    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats, in CharacterMovement movement, 
        in Animator opponentAnimator, in Transform opponentTransform, in Rigidbody rb, in Collider col)
    {
        moveList = stats.MoveList;
        hitstop.Reference(opponentAnimator, opponentTransform, animator, rb, col);

        movement.OnMoveCharacter += MovementAnimation;

        stateMachine.WalkingState.OnEnter += () => animator.SetBool("STATE_WALKING", true);
        stateMachine.WalkingState.OnExit += () => animator.SetBool("STATE_WALKING", false);

        stateMachine.BlockingState.OnEnter += () => animator.SetBool("STATE_BLOCKING", true);
        stateMachine.BlockingState.OnExit += () => animator.SetBool("STATE_BLOCKING", false);

        stateMachine.MoveState.OnEnterInteger += (int moveIndex) => animator.SetTrigger(moveList[moveIndex].AnimatorTrigger);

        stateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => {
            animator.SetBool("STATE_HURT", true);
            TriggerHurtAnimation(hitbox.HurtSide, hitbox.HurtHeight, hitbox.HurtPower);
            hitstop.Trigger(hitbox.HurtHitStop);
        };
        stateMachine.HurtState.OnExit += () => animator.SetBool("STATE_HURT", false);

        stats.OnHyperarmorHurt += (in Hitbox hitbox) => hitstop.Trigger(hitbox.HurtHitStop);

        stateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => {
            animator.SetBool("STATE_BLOCKED", true);
            TriggerHurtAnimation(hitbox.HurtSide, hitbox.HurtHeight, hitbox.HurtPower);
            hitstop.Trigger(hitbox.BlockHitStop);
        };
        stateMachine.BlockedState.OnExit += () => animator.SetBool("STATE_BLOCKED", false);

        stateMachine.StaggerState.OnEnterHitbox += (in Hitbox hitbox) => {
            animator.SetBool("STATE_STAGGER", true);
            TriggerHurtAnimation(hitbox.HurtSide, hitbox.HurtHeight, hitbox.HurtPower);
            hitstop.Trigger(hitbox.StaggerHitStop);
        };
        stateMachine.StaggerState.OnExit += () => animator.SetBool("STATE_STAGGER", false);

        stateMachine.KOState.OnEnterHitbox += (in Hitbox hitbox) => {
            animator.SetFloat("hurt_side", hitbox.HurtSide);
            animator.SetFloat("hurt_height", hitbox.HurtHeight);
            animator.SetFloat("hurt_power", hitbox.HurtPower);
            animator.SetBool("STATE_KO", true);
            hitstop.Trigger(hitbox.KOHitStop);
        };
        stateMachine.KOState.OnExit += () => animator.SetBool("STATE_KO", false);
    }

    public void OnValidate()
    {
        if (animator != null) animator.speed = animatorSpeed;
    }

    private void MovementAnimation(in Vector2 direction)
    {
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }

    private void TriggerHurtAnimation(float side, float height, float power)
    {
        animator.SetFloat("hurt_side", side);
        animator.SetFloat("hurt_height", height);
        animator.SetFloat("hurt_power", power);
        animator.SetTrigger("hurt");
    }

    public ref readonly HitStop HitStop => ref hitstop;
}