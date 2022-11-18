using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private AnimatorOverrideController animOverride;
    private AnimationClip[] animatorDefaults;

    private Vector2 movementValue, direction;
    private bool isAttacking, isBlocking, canAttack;

    public Transform TargetEnemy;

    [Header("Animation Parameters")]
    public float generalSpeed = 1f;
    [Range(0f, 1f)] public float load = 0f;
    public float dodgeSpeed = 1f;

    [Header("Movement Parameters")]
    public float movementSpeed = 8f;
    [Range(0f, 1f)] public float attackingModifier = 0f, blockingModifier = 0f; // The player may move slower when attacking or blocking.
    private float current_movementSpeed;
    [Range(-1f, 0f)] public float duckingRange = -1f; // -1: can duck all the way down. 0: can't duck at all.

    [Header("Attack Parameters")]
    // The player has four attack slots to define their moveset.
    // Two attacks from the left (left arm, left leg), two attacks from the right.
    public Move leftNormalSlot;
    public Move rightNormalSlot;
    public Move leftSpecialSlot;
    public Move rightSpecialSlot;
    public float attackCooldown = 0f; // Time before the player can attack again.

    private void Awake()
    {
        anim = GetComponent<Animator>();
        animatorDefaults = anim.runtimeAnimatorController.animationClips;
        animOverride = new AnimatorOverrideController(anim.runtimeAnimatorController);
    }

    private void Start()
    {
        movementValue = Vector2.zero;
        direction = Vector2.zero;
        isAttacking = false;
        isBlocking = false;
        canAttack = true;
        UpdateAllAttackAnimations();
    }

    void Update()
    {
        SetAnimationParameters();
    }

    //***INPUT***
    // Meant for Unity Input System events

    public void Movement(InputAction.CallbackContext context)
    { 
        movementValue = context.ReadValue<Vector2>();
        movementValue.y = Mathf.Clamp(movementValue.y, duckingRange, 0f); // -1 is crouching, 0 is standing. Doesn't make sense to consider 1 as a value.
    }

    public void LeftNormal(InputAction.CallbackContext context) { anim.SetBool("left_normal", context.performed); }
    public void RightNormal(InputAction.CallbackContext context) { anim.SetBool("right_normal", context.performed); }
    public void LeftSpecial(InputAction.CallbackContext context) { anim.SetBool("left_special", context.performed); }
    public void RightSpecial(InputAction.CallbackContext context) { anim.SetBool("right_special", context.performed); }
    public void Block(InputAction.CallbackContext context) { anim.SetBool("block", context.performed); }
    public void Dodge(InputAction.CallbackContext context) { anim.SetBool("dodge", context.performed); }

    //***ANIMATION***

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
    /// Update left normal slot animations in real time.
    /// </summary>
    public void UpdateLeftNormalAnimations()
    {
        UpdateAnimator("LeftNormalCrouchClip", leftNormalSlot.crouchLeftAnimation);
        UpdateAnimator("LeftNormalClip", leftNormalSlot.leftAnimation);
    }

    /// <summary>
    /// Update right normal slot animations in real time.
    /// </summary>
    public void UpdateRightNormalAnimations()
    {
        UpdateAnimator("RightNormalCrouchClip", rightNormalSlot.crouchRightAnimation);
        UpdateAnimator("RightNormalClip", rightNormalSlot.rightAnimation);
    }

    /// <summary>
    /// Update left special slot animations in real time.
    /// </summary>
    public void UpdateLeftSpecialAnimations()
    {
        UpdateAnimator("LeftSpecialCrouchClip", leftSpecialSlot.crouchLeftAnimation);
        UpdateAnimator("LeftSpecialClip", leftSpecialSlot.leftAnimation);
    }

    /// <summary>
    /// Update right special slot animations in real time.
    /// </summary>
    public void UpdateRightSpecialAnimations()
    {
        UpdateAnimator("RightSpecialCrouchClip", rightSpecialSlot.crouchRightAnimation);
        UpdateAnimator("RightSpecialClip", rightSpecialSlot.rightAnimation);
    }
    
    /// <summary>
    /// Updates all attack animations in real time.
    /// </summary>
    public void UpdateAllAttackAnimations()
    {
        UpdateLeftNormalAnimations();
        UpdateRightNormalAnimations();
        UpdateLeftSpecialAnimations();
        UpdateRightSpecialAnimations();
    }

    /// <summary>
    /// Sets general animation parameters for the animator.
    /// </summary>
    private void SetAnimationParameters()
    {
        // Values that must be updated frame by frame to allow certain animations to play out accordingly.
        isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && !anim.IsInTransition(0);
        isBlocking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Blocking") && !anim.IsInTransition(0);
        // The player can attack if the attack animation hasn't been playing for less than *attackCooldown* seconds.
        canAttack = !(isAttacking && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < attackCooldown));
        anim.SetBool("can_attack", canAttack);
        
        // Animation modifiers
        anim.SetFloat("load", load);
        anim.SetFloat("left_normal_speed", leftNormalSlot.animationSpeed * generalSpeed);
        anim.SetFloat("right_normal_speed", rightNormalSlot.animationSpeed * generalSpeed);
        anim.SetFloat("left_special_speed", leftSpecialSlot.animationSpeed * generalSpeed);
        anim.SetFloat("right_special_speed", rightSpecialSlot.animationSpeed * generalSpeed);
        anim.SetFloat("dodge_speed", dodgeSpeed * generalSpeed);

        // MOVEMENT
        // Softens the movement by establishing the direction as a point that approaches the stick/mouse position.
        current_movementSpeed = movementSpeed - movementSpeed * attackingModifier * System.Convert.ToSingle(isAttacking)
            - movementSpeed * blockingModifier * System.Convert.ToSingle(isBlocking);
        direction = Vector2.MoveTowards(direction, movementValue, current_movementSpeed * Time.deltaTime);
        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);
        transform.LookAt(new Vector3(TargetEnemy.position.x, transform.position.y, TargetEnemy.position.z)); // Rotate towards enemy.
    }

    //***PUBLIC METHODS***

    public Animator getAnimator { get { return anim; } }

    public bool isWalking { get { return direction.x != 0f && !isAttacking; } }
}
