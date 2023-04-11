using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterAnimationHandler : MonoBehaviour
{
    private Character logic;
    private Animator anim;
    private AnimatorOverrideController animOverride;
    private AnimationClip[] animatorDefaults;

    [SerializeField] private bool updateMoveAnimations = false, updateMoveTimeData = false;

    private void Awake()
    {
        logic = GetComponent<Character>();
        anim = GetComponent<Animator>();
        animatorDefaults = anim.runtimeAnimatorController.animationClips;
        animOverride = new AnimatorOverrideController(anim.runtimeAnimatorController);
    }
    private void Start()
    {
        UpdateMovesetAnimations();

        logic.OnAttackPerformed += TriggerAttackAnimation;
        logic.OnDamage += TriggerHurtAnimation;
    }

    private void OnValidate()
    {
        if (updateMoveAnimations) { UpdateMovesetAnimations(); updateMoveAnimations = false; }
        if (updateMoveTimeData) { foreach (Move m in logic.Moveset) { m.AssignEvents(); } updateMoveTimeData = false; }
    }

    private void Update()
    {
        anim.SetBool("can_attack", logic.CanAttack);
        anim.SetBool("is_blocking", logic.IsBlocking);
        anim.SetBool("ko", logic.State == CharacterState.KO);
        anim.SetFloat("horizontal", logic.Direction.x);
        anim.SetFloat("vertical", logic.Direction.y);
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
        anim.runtimeAnimatorController = animOverride;
    }

    /// <summary>
    /// Assigns moves' animations and speed to animator.
    /// </summary>
    private void UpdateMovesetAnimations()
    {
        for (int i = 0; i < logic.Moveset.Count; ++i)
        {
            UpdateAnimator("AttackClip" + i, logic.Moveset[i].Animation);
            anim.SetFloat("attack" + i + "_speed", logic.Moveset[i].AnimationSpeed);
        }
    }

    #endregion

    #region AnimationEvents

    public event UnityAction OnAttackStart, OnAttackActive, OnAttackRecovery,
        OnAnimationCancel;

    public void AttackStart()
    {
        anim.ResetTrigger("cancel");
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
        anim.SetTrigger("cancel");
    }
    private IEnumerator WaitAndCancelAnimation(float ms)
    {
        StopAllCoroutines();
        yield return new WaitForSeconds(ms / 1000f);
        AnimationCancel();
    }

    #endregion

    #region AnimationTriggers
    public void TriggerAttackAnimation() { anim.SetTrigger("attack" + logic.currentIndex); }
    public void TriggerHurtAnimation()
    {
        anim.SetTrigger("hurt");
        anim.SetFloat("hurt_target", logic.HurtTarget);
        anim.SetFloat("hurt_power", logic.HurtPower);
        StartCoroutine(WaitAndCancelAnimation(logic.Disadvantage));
    }
    #endregion
}
