using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CharacterAnimation
{
    private List<Move> moveList;

    private Animator animator;
    private AnimatorOverrideController animatorOverride;
    private AnimationClip[] animatorDefaults;

    [SerializeField] private float animatorSpeed = 1f;
    [SerializeField] private bool updateMovesetAnimations = false;

    public void Initialize(in Animator animator)
    {
        this.animator = animator;
        animatorDefaults = animator.runtimeAnimatorController.animationClips;
        animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
    }
    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats, in CharacterMovement movement)
    {
        moveList = stats.MoveList;
        UpdateMovesetAnimations();

        HurtState hurtState = stateMachine.HurtState;
        BlockedState blockedState = stateMachine.BlockedState;
        KOState koState = stateMachine.KOState;
        StaggerState staggerState = stateMachine.StaggerState;

        movement.OnMoveCharacter += MovementAnimation;

        stateMachine.WalkingState.OnEnter += () => animator.SetBool("STATE_WALKING", true);
        stateMachine.WalkingState.OnExit += () => animator.SetBool("STATE_WALKING", false);

        stateMachine.BlockingState.OnEnter += () => animator.SetBool("STATE_BLOCKING", true);
        stateMachine.BlockingState.OnExit += () => animator.SetBool("STATE_BLOCKING", false);

        stateMachine.MoveState.OnEnterInteger += (int moveIndex) => animator.SetTrigger("move" + moveIndex);

        stateMachine.HurtState.OnEnter += () => {
            Hitbox hitbox = hurtState.Hitbox;
            animator.SetBool("STATE_HURT", true);
            TriggerHurtAnimation(hitbox.AnimationBodyTarget, hitbox.HurtLevel);
        };
        stateMachine.HurtState.OnExit += () => animator.SetBool("STATE_HURT", false);

        stateMachine.BlockedState.OnEnter += () => {
            Hitbox hitbox = blockedState.Hitbox;
            animator.SetBool("STATE_BLOCKED", true);
            TriggerHurtAnimation(hitbox.AnimationBodyTarget, hitbox.HurtLevel);
        };
        stateMachine.BlockedState.OnExit += () => animator.SetBool("STATE_BLOCKED", false);

        stateMachine.KOState.OnEnter += () => {
            Hitbox hitbox = koState.Hitbox;
            animator.SetFloat("hurt_target", hitbox.AnimationBodyTarget);
            animator.SetFloat("hurt_power", hitbox.HurtLevel);
            animator.SetBool("STATE_KO", true);
        };
        stateMachine.KOState.OnExit += () => animator.SetBool("STATE_KO", false);

        stateMachine.StaggerState.OnEnter += () =>
        {
            Hitbox hitbox = staggerState.Hitbox;
            animator.SetBool("STATE_STAGGER", true);
            TriggerHurtAnimation(hitbox.AnimationBodyTarget, hitbox.HurtLevel);
        };
        stateMachine.StaggerState.OnExit += () => animator.SetBool("STATE_STAGGER", false);
    }
    public void OnValidate()
    {
        if (animator != null) animator.speed = animatorSpeed;
        if (updateMovesetAnimations) { UpdateMovesetAnimations(); updateMovesetAnimations = false; }
    }

    private void UpdateAnimation(string originalClip, AnimationClip newClip)
    {
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(animatorDefaults.Where(clip => clip.name == originalClip).SingleOrDefault(), newClip));
        animatorOverride.ApplyOverrides(overrides);
        animator.runtimeAnimatorController = animatorOverride;
    }

    private void UpdateMovesetAnimations()
    {
        for (int i = 0; i < moveList.Count; ++i)
        {
            UpdateAnimation("MoveClip" + i, moveList[i].Animation);
            animator.SetFloat("move" + i + "_speed", moveList[i].AnimationSpeed);
        }
    }

    private void MovementAnimation(in Vector2 direction)
    {
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }

    private void TriggerHurtAnimation(float target, float hurtLevel)
    {
        animator.SetFloat("hurt_target", target);
        animator.SetFloat("hurt_power", hurtLevel);
        animator.SetTrigger("hurt");
    }
}