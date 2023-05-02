using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterAnimation : MonoBehaviour
{
    private Character character;
    private Animator animator;
    private AnimatorOverrideController animatorOverride;
    private AnimationClip[] animatorDefaults;
    [SerializeField] private float animatorSpeed = 1f;
    [SerializeField] private bool updateAnimations = false;

    public void Initialize()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        animatorDefaults = animator.runtimeAnimatorController.animationClips;
        animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
        UpdateMovesetAnimations();

        character.Movement.OnMoveCharacter += MoveAnimation;

        character.StateMachine.WalkingState.OnEnter += EnterWalkingState;
        character.StateMachine.WalkingState.OnExit += ExitWalkingState;

        character.StateMachine.BlockingState.OnEnter += EnterBlockingState;
        character.StateMachine.BlockingState.OnExit += ExitBlockingState;

        character.StateMachine.AttackingState.OnEnter += EnterAttackingState;

        character.StateMachine.HurtState.OnEnter += EnterHurtState;
        character.StateMachine.HurtState.OnExit += ExitHurtState;

        character.StateMachine.BlockedState.OnEnter += EnterBlockedState;
        character.StateMachine.BlockedState.OnExit += ExitBlockedState;
    }

    private void OnValidate()
    {
        if (animator != null) animator.speed = animatorSpeed;
        if (updateAnimations) { UpdateMovesetAnimations(); updateAnimations = false; }
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
        for (int i = 0; i < character.Stats.MoveList.Count; ++i)
        {
            UpdateAnimation("MoveClip" + i, character.Stats.MoveList[i].Animation);
            animator.SetFloat("move" + i + "_speed", character.Stats.MoveList[i].AnimationSpeed);
        }
    }

    private void MoveAnimation(in Vector2 direction)
    {
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }

    private void TriggerHurtAnimation(float target, float stagger)
    {
        animator.SetFloat("hurt_target", target);
        animator.SetFloat("hurt_stagger", stagger);
        animator.SetTrigger("hurt");
    }

    #region Animation Events

    private void InitMove() 
    {
        character.Stats.MoveList[character.StateMachine.AttackingState.moveIndex].InitMove();
    }
    private void ActivateMove() 
    {
        character.Stats.MoveList[character.StateMachine.AttackingState.moveIndex].ActivateMove();
    }
    private void DeactivateMove() 
    {
        character.Stats.MoveList[character.StateMachine.AttackingState.moveIndex].DeactivateMove();
    }
    private void RecoverFromMove()
    {
        character.Stats.MoveList[character.StateMachine.AttackingState.moveIndex].RecoverFromMove();
        character.StateMachine.TransitionToMovement.Invoke();
    }

    #endregion

    #region Enter/Exit States

    private void EnterWalkingState() { animator.SetBool("STATE_WALKING", true); }
    private void ExitWalkingState() { animator.SetBool("STATE_WALKING", false); }

    private void EnterBlockingState() { animator.SetBool("STATE_BLOCKING", true); }
    private void ExitBlockingState() { animator.SetBool("STATE_BLOCKING", false); }

    private void EnterHurtState() 
    {
        IHit hitbox = character.StateMachine.HurtState.Hitbox;
        animator.SetBool("STATE_HURT", true);
        TriggerHurtAnimation(hitbox.Target, hitbox.Stagger);
    }
    private void ExitHurtState()
    {
        animator.SetBool("STATE_HURT", false);
    }

    private void EnterBlockedState()
    {
        IBlocked hitbox = character.StateMachine.BlockedState.Hitbox;
        animator.SetBool("STATE_BLOCKED", true);
        TriggerHurtAnimation(hitbox.Target, hitbox.Stagger);
    }
    private void ExitBlockedState()
    {
        animator.SetBool("STATE_BLOCKED", false);
    }

    private void EnterAttackingState(int moveIndex) { animator.SetTrigger("move" + moveIndex); }

    #endregion
}
