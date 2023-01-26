using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    private Vector2 movementValue, direction;
    private bool isAttacking, isBlocking, canAttack, canBlock;

    [Header("Movement Parameters")]
    [Tooltip("How quickly player character follows stick movement")] 
    [SerializeField] private float movementSpeed = 8f;
    private float current_movementSpeed;

    [Tooltip("Modifies player movement speed when attacking or blocking (the player may move slower, or not move at all)")]
    [SerializeField] [Range(0f, 1f)] private float attackingModifier = 0f, blockingModifier = 0f;

    [SerializeField] [Range(-1f, 0f)] private float duckingRange = -1f; // -1: can duck all the way down. 0: can't duck at all.

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

    [Tooltip("(Normalized) Range of time where the player can cancel an attack to block")]
    [SerializeField] [Range(0f, 1f)] private float cancelAttackTime = 0.4f;
    [System.NonSerialized] public bool cancelable = true;

    private void Awake()
    {
        init();
    }

    private void Start()
    {
        movementValue = Vector2.zero;
        direction = Vector2.zero;
        isAttacking = false;
        isBlocking = false;
        canAttack = true;
        canBlock = false;
        UpdateAllAttackAnimations();
    }

    void Update()
    {
        SetAnimationParameters();
        updating();
    }

    //***INPUT***
    // Meant for Unity Input System events

    public void Movement(InputAction.CallbackContext context)
    { 
        movementValue = context.ReadValue<Vector2>();
        movementValue.y = Mathf.Clamp(movementValue.y, duckingRange, 1f); // -1 is crouching, 0 is standing, 1 is moving forward.
    }

    public void LeftNormal(InputAction.CallbackContext context) { leftNormalSlot.pressed = context.performed; anim.SetBool("left_normal", context.performed); }
    public void RightNormal(InputAction.CallbackContext context) { rightNormalSlot.pressed = context.performed; anim.SetBool("right_normal", context.performed); }
    public void LeftSpecial(InputAction.CallbackContext context) { leftSpecialSlot.pressed = context.performed; anim.SetBool("left_special", context.performed); }
    public void RightSpecial(InputAction.CallbackContext context) { rightSpecialSlot.pressed = context.performed; anim.SetBool("right_special", context.performed); }
    public void Block(InputAction.CallbackContext context) { anim.SetBool("block", context.performed); }

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
        isBlocking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Blocking") && !anim.IsInTransition(0);
        // The player can't attack if the attack animation has been playing for less than *interAttackExitTime* and...
        canAttack = !(isAttacking && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < interAttackExitTime)) && !isBlocking;
        // The player can't block if the attack animation has been playing for more than *cancelAttackTime* or if the attack is uncancellable
        canBlock = !(isAttacking && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= cancelAttackTime || !cancelable));
        anim.SetBool("can_attack", canAttack);
        anim.SetBool("can_block", canBlock);

        // Animation modifiers
        anim.SetFloat("left_normal_speed", leftNormalSlot.leftAnimationSpeed * leftNormalSlot.chargeSpeed * attackSpeed);
        anim.SetFloat("right_normal_speed", rightNormalSlot.rightAnimationSpeed * rightNormalSlot.chargeSpeed * attackSpeed);
        anim.SetFloat("left_special_speed", leftSpecialSlot.leftAnimationSpeed * leftSpecialSlot.chargeSpeed * attackSpeed);
        anim.SetFloat("right_special_speed", rightSpecialSlot.rightAnimationSpeed * rightSpecialSlot.chargeSpeed * attackSpeed);

        // MOVEMENT
        // Softens the movement by establishing the direction as a point that approaches the stick/mouse position.
        current_movementSpeed = movementSpeed - movementSpeed * attackingModifier * System.Convert.ToSingle(isAttacking)
            - movementSpeed * blockingModifier * System.Convert.ToSingle(isBlocking);
        direction = Vector2.MoveTowards(direction, movementValue, current_movementSpeed * Time.deltaTime);
        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);
    }

    //***GET FUNCTIONS***

    public bool isWalking { get { return direction.x != 0f && !isAttacking; } }
}
