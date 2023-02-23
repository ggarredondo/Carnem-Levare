using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private bool isAttacking, isHurt, isSkipping;

    [Header("Input Parameters")]

    [Tooltip("How quickly player animations follows stick movement")]
    [SerializeField] private float stickSpeed;

    [Tooltip("Lower stickSpeed to smooth out transitions to idle (when stick is centered)")]
    [SerializeField] private float smoothStickSpeed;

    [Tooltip("How much off axis movement do you allow when attempting stick tapping actions?")]
    [SerializeField] private float stickTapTolerance = 0.1f;
    private bool canTapStick = true;

    override protected void Update()
    {
        // Bellow are values that must be updated frame by frame to allow certain animations to play out accordingly.
        isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && !anim.IsInTransition(0);
        isHurt = anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt") && !anim.IsInTransition(0);

        // The player can only attack if they're not attacking already.
        anim.SetBool("can_attack", !isAttacking && !isHurt);

        // The player can only skip if they are blocking but they aren't attacking nor skipping already.
        isSkipping = anim.GetCurrentAnimatorStateInfo(0).IsTag("Skipping");
        // Attacking is checked through the animator so that you can buffer skip after attacking.
        // Everything else is checked through input so that skipping doesn't buffer for the next frames.
        anim.SetBool("can_skip", !isAttacking && !isSkipping && !isHurt);

        directionSpeed = directionTarget.magnitude == 0f && !isBlocking ? smoothStickSpeed : stickSpeed;
        base.Update();
    }

    #region Input
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
    public void SkipFwd(InputAction.CallbackContext context) { anim.SetBool("skip_fwd", context.performed && isBlocking && canTapStick); }
    public void SkipBwd(InputAction.CallbackContext context) { anim.SetBool("skip_bwd", context.performed && isBlocking && canTapStick); }
    public void LeftNormal(InputAction.CallbackContext context) { if (LeftMoveset.Count > 0) anim.SetBool("left_normal", context.performed); }
    public void LeftSpecial(InputAction.CallbackContext context) { if (LeftMoveset.Count > 1) anim.SetBool("left_special", context.performed); }
    public void RightNormal(InputAction.CallbackContext context) {
        if (RightMoveset.Count > 0) {
            PressMove(0, context.performed);
            anim.SetBool("right_normal", context.performed);
        }
    }
    public void RightSpecial(InputAction.CallbackContext context) {
        if (RightMoveset.Count > 1) {
            PressMove(1, context.performed);
            anim.SetBool("right_special", context.performed);
        }
    }
    public void Block(InputAction.CallbackContext context) { isBlocking = context.performed; }
    #endregion

    #region PublicMethods
    /// <summary>
    /// Return's player current stick position.
    /// </summary>
    public Vector2 StickDirection { get => directionTarget; }

    /// <summary>
    /// Returns player's linearly interpolated stick direction.
    /// </summary>
    public Vector2 StickSmoothDirection { get => direction; }

    /// <summary>
    /// Checks if player is moving, whether by walking or blocking.
    /// </summary>
    public bool isPlayerMoving { get => directionTarget.magnitude != 0f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement"); }
    public bool isPlayerIdle { get => directionTarget.magnitude == 0f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement"); }
    public bool isPlayerSkippingForward { get => directionTarget.magnitude == 0f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement"); }
    public bool isPlayerSkippingBackwards { get => anim.GetCurrentAnimatorStateInfo(0).IsName("Skip Backwards"); }
    public bool isPlayerAttacking { get => anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking"); }
    public bool isPlayerBlocking { get => isBlocking; }
    #endregion
}
