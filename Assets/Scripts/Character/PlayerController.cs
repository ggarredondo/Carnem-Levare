using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    private Vector2 directionTarget, direction;
    private bool isAttacking, isBlocking, isSkipping, canAttack, canSkip;

    [Header("Attack Parameters")]
    // The player has four attack slots to define their moveset.
    // Two attacks from the left (left arm, left leg), two attacks from the right.
    public Move leftNormalSlot;
    public Move rightNormalSlot;
    public Move leftSpecialSlot;
    public Move rightSpecialSlot;

    // 0 means the player can attack again immediately. 1 means the player must wait for the entire animation to play out.
    // 0.5 means the player must wait for half the attack animation to play out before attacking again. Etc.
    // This variable is meant for transitions between different move slots. Move slots can't transition directly to themselves,
    // spamming the same move will always require the entire animation to play out.
    [Tooltip("(Normalized) Time before the player can attack again between different moves")]
    [SerializeField] [Range(0f, 1f)] private float interAttackExitTime = 0.4f;

    [Header("Input Parameters")]
    [Tooltip("How quickly player animations follows stick movement")]
    [SerializeField] private float stickSpeed;

    [Tooltip("Lower stickSpeed to smooth out transitions to idle (when stick is centered)")]
    [SerializeField] private float smoothStickSpeed;
    private float finalStickSpeed;

    [Tooltip("How much off axis movement do you allow when attempting stick tapping actions?")]
    [SerializeField] private float stickTapTolerance = 0.1f;
    private bool canTapStick = true;

    [Header("Debug")]
    [SerializeField] private bool updateAnimations = false;
    [SerializeField] private bool modifyTimeScale = false;
    [SerializeField] [Range(0f, 1f)] private float timeScale = 1f;

    private void Awake()
    {
        init();
    }

    private void Start()
    {
        directionTarget = Vector2.zero;
        direction = Vector2.zero;
        isAttacking = false;
        isBlocking = false;
        canAttack = true;
        UpdateAllAttackAnimations();
    }

    void Update()
    {
        SetAnimationParameters();
        if (modifyTimeScale) Time.timeScale = timeScale; // DEBUG
        if (updateAnimations) { // DEBUG
            UpdateAllAttackAnimations(); // DEBUG
            updateAnimations = false; // DEBUG
        }
    }

    private void FixedUpdate()
    {
        fixedUpdating();
    }

    //***INPUT***
    // Meant for Unity Input System events

    public void Movement(InputAction.CallbackContext context) { 
        directionTarget = context.ReadValue<Vector2>().normalized;

        // Forward and backwards skips trigger by tapping the left stick twice (up or down respectively).
        // However, they may also trigger by drawing circles in the stick. We have to make sure there is
        // no horizontal movement before allowing the player to skip jump, otherwise they may trigger it
        // by mistake when dodging attacks.
        // We, however, allow some horizontal movement so that it doesn't have to be too precise.
        // In the range of [-stickTapTolerance, stickTapTolerance]
        if (directionTarget.x < -stickTapTolerance || directionTarget.x > stickTapTolerance) canTapStick = false;
        if (directionTarget.magnitude == 0f) canTapStick = true;
    }

    public void SkipFwd(InputAction.CallbackContext context) { anim.SetBool("skip_fwd", context.performed && canSkip); }
    public void SkipBwd(InputAction.CallbackContext context) { anim.SetBool("skip_bwd", context.performed && canSkip); }
    public void LeftNormal(InputAction.CallbackContext context) { leftNormalSlot.pressed = context.performed; anim.SetBool("left_normal", context.performed); }
    public void RightNormal(InputAction.CallbackContext context) { rightNormalSlot.pressed = context.performed; anim.SetBool("right_normal", context.performed); }
    public void LeftSpecial(InputAction.CallbackContext context) { leftSpecialSlot.pressed = context.performed; anim.SetBool("left_special", context.performed); }
    public void RightSpecial(InputAction.CallbackContext context) { rightSpecialSlot.pressed = context.performed; anim.SetBool("right_special", context.performed); }
    public void Block(InputAction.CallbackContext context) { isBlocking = context.performed; }

    //***ANIMATION***

    /// <summary>
    /// Updates all attack animations in real time.
    /// </summary>
    public void UpdateAllAttackAnimations()
    {
        UpdateAnimator("LeftNormalClip", leftNormalSlot.leftAnimation);
        UpdateAnimator("RightNormalClip", rightNormalSlot.rightAnimation);
        UpdateAnimator("LeftSpecialClip", leftSpecialSlot.leftAnimation);
        UpdateAnimator("RightSpecialClip", rightSpecialSlot.rightAnimation);
    }

    /// <summary>
    /// Sets animation parameters for the animator.
    /// </summary>
    private void SetAnimationParameters()
    {
        // Values that must be updated frame by frame to allow certain animations to play out accordingly.
        isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && !anim.IsInTransition(0);
        isSkipping = anim.GetCurrentAnimatorStateInfo(0).IsTag("Skipping") && !anim.IsInTransition(0);

        // The player can't attack if the attack animation has been playing for less than *interAttackExitTime* or if they are skipping.
        canAttack = !((isAttacking && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < interAttackExitTime) || isSkipping);
        anim.SetBool("can_attack", canAttack);

        // The play can only skip if they are blocking and they aren't attacking or skipping already.
        canSkip = isBlocking && canTapStick && !isAttacking && !isSkipping;

        // Animation modifiers
        anim.SetFloat("left_normal_speed", leftNormalSlot.leftAnimationSpeed * attackSpeed * leftNormalSlot.chargeSpeed);
        anim.SetFloat("right_normal_speed", rightNormalSlot.rightAnimationSpeed * attackSpeed * rightNormalSlot.chargeSpeed);
        anim.SetFloat("left_special_speed", leftSpecialSlot.leftAnimationSpeed * attackSpeed * leftSpecialSlot.chargeSpeed);
        anim.SetFloat("right_special_speed", rightSpecialSlot.rightAnimationSpeed * attackSpeed * rightSpecialSlot.chargeSpeed);

        anim.SetBool("block", isBlocking);
        // Ternary operator so that when the player isn't moving, the speed parameter doesn't affect the idle animation
        anim.SetFloat("casual_walk_speed", directionTarget.magnitude == 0f ? 1f : casualWalkingSpeed);
        anim.SetFloat("guard_walk_speed", directionTarget.magnitude == 0f ? 1f : guardWalkingSpeed);

        finalStickSpeed = directionTarget.magnitude == 0f && !isBlocking ? smoothStickSpeed : stickSpeed;
        // Softens the stick movement by establishing the direction as a point that approaches the stick/mouse position at *finalStickSpeed* rate.
        direction = Vector2.Lerp(direction, directionTarget, finalStickSpeed * Time.deltaTime);
        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);
    }

    //***GET FUNCTIONS***

    /// <summary>
    /// Is the player in any State tagged as "Movement"?
    /// </summary>
    public bool getIsMovement { get { return anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement"); } }

    /// <summary>
    /// Is the player moving? The player may move by pressing the stick or by attacking.
    /// </summary>
    public bool getIsMoving { get { return !(directionTarget.magnitude == 0f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement")); } }

    public float getDirectionX { get { return direction.x; } }

    public bool getIsBlocking { get { return isBlocking; } }
}
