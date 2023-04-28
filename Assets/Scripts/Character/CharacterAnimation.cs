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

        character.WalkingState.OnEnter += EnterWalkingState;
        character.WalkingState.OnExit += ExitWalkingState;

        character.BlockingState.OnEnter += EnterBlockingState;
        character.BlockingState.OnExit += ExitBlockingState;

        character.AttackingState.OnEnter += EnterAttackingState;
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
        for (int i = 0; i < character.MoveList.Count; ++i)
        {
            UpdateAnimation("MoveClip" + i, character.MoveList[i].Animation);
            animator.SetFloat("move" + i + "_speed", character.MoveList[i].AnimationSpeed);
        }
    }

    private void MoveAnimation(in Vector2 direction)
    {
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }

    #region Animation Events

    private void InitMove() 
    {
        character.MoveList[character.AttackingState.moveIndex].InitMove();
    }
    private void ActivateMove() 
    {
        character.MoveList[character.AttackingState.moveIndex].ActivateMove();
    }
    private void DeactivateMove() 
    {
        character.MoveList[character.AttackingState.moveIndex].DeactivateMove();
    }
    private void RecoverFromMove()
    {
        character.MoveList[character.AttackingState.moveIndex].RecoverFromMove();
        character.ChangeState(character.WalkingState);
    }

    #endregion

    #region Enter/Exit States

    private void EnterWalkingState() { animator.SetBool("STATE_WALKING", true); }
    private void ExitWalkingState() { animator.SetBool("STATE_WALKING", false); }

    private void EnterBlockingState() { animator.SetBool("STATE_BLOCKING", true); }
    private void ExitBlockingState() { animator.SetBool("STATE_BLOCKING", false); }

    private void EnterAttackingState(int moveIndex) { animator.SetTrigger("move" + moveIndex); }

    #endregion
}
