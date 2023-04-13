using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterAnimation : MonoBehaviour
{
    private CharacterLogic logicHandler;
    private Animator animator;
    private AnimatorOverrideController animOverride;
    private AnimationClip[] animatorDefaults;

    [SerializeField] private bool updateMoveAnimations = false, updateMoveTimeData = false;

    private void Awake()
    {
        logicHandler = GetComponent<CharacterLogic>();
        animator = GetComponent<Animator>();
        animatorDefaults = animator.runtimeAnimatorController.animationClips;
        animOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
    }
    private void Start()
    {
        UpdateMovesetAnimations();
        logicHandler.OnAttackPerformed += TriggerAttackAnimation;
    }

    private void OnValidate()
    {
        if (updateMoveAnimations) { UpdateMovesetAnimations(); updateMoveAnimations = false; }
        if (updateMoveTimeData) { foreach (Move m in logicHandler.Moveset) { m.AssignEvents(); } updateMoveTimeData = false; }
    }

    private void Update()
    {
        animator.SetBool("can_attack", logicHandler.CanAttack);
        animator.SetFloat("horizontal", logicHandler.Direction.x);
        animator.SetFloat("vertical", logicHandler.Direction.y);

        animator.SetBool("STATE_WALKING", logicHandler.State == CharacterState.WALKING);
        animator.SetBool("STATE_BLOCKING", logicHandler.State == CharacterState.BLOCKING);
        animator.SetBool("STATE_HURT", logicHandler.State == CharacterState.HURT);
        animator.SetBool("STATE_BLOCKED", logicHandler.State == CharacterState.BLOCKED);
        animator.SetBool("STATE_KO", logicHandler.State == CharacterState.KO);
    }

    #region AnimatorOverride

    /// <summary>
    /// Updates specific animation from animator in real time.
    /// </summary>
    /// <param name="og_clip">Name of the animation clip to be updated</param>
    /// <param name="new_clip">New animation clip</param>
    private void UpdateAnimator(string og_clip, AnimationClip new_clip)
    {
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(animatorDefaults.Where(clip => clip.name == og_clip).SingleOrDefault(), new_clip));
        animOverride.ApplyOverrides(overrides);
        animator.runtimeAnimatorController = animOverride;
    }

    /// <summary>
    /// Assigns moves' animations and speed to animator.
    /// </summary>
    private void UpdateMovesetAnimations()
    {
        for (int i = 0; i < logicHandler.Moveset.Count; ++i)
        {
            UpdateAnimator("AttackClip" + i, logicHandler.Moveset[i].Animation);
            animator.SetFloat("attack" + i + "_speed", logicHandler.Moveset[i].AnimationSpeed);
        }
    }

    #endregion

    #region AnimationEvents

    public event UnityAction OnAttackStart, OnAttackActive, OnAttackRecovery,
        OnAnimationCancel;

    public void AttackStart()
    {
        animator.ResetTrigger("cancel");
        OnAttackStart.Invoke();
    }
    public void AttackActive()
    {
        OnAttackActive.Invoke();
    }
    public void AttackRecovery()
    {
        OnAttackRecovery.Invoke();
    }
    public void AnimationCancel()
    {
        OnAnimationCancel.Invoke();
        animator.SetTrigger("cancel");
    }
    private IEnumerator WaitAndCancelAnimation(float ms)
    {
        yield return new WaitForSeconds(ms / 1000f);
        AnimationCancel();
    }

    #endregion

    #region AnimationTriggers

    public void TriggerAttackAnimation() { animator.SetTrigger("attack" + logicHandler.currentIndex); }
    public void TriggerHurtAnimation(float target, float power)
    {
        animator.SetTrigger("hurt");
        animator.SetFloat("hurt_target", target);
        animator.SetFloat("hurt_power", power);
        StopAllCoroutines();
        StartCoroutine(WaitAndCancelAnimation(logicHandler.Disadvantage));
    }

    #endregion
}
