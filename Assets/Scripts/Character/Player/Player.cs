using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private bool isDashing;

    [Header("Input Parameters")]

    [Tooltip("How quickly player animations follows stick movement")]
    [SerializeField] private float stickSpeed;

    [Tooltip("How quickly you can duck and sidestep when blocking")]
    [SerializeField] private float blockingStickSpeed;

    [Tooltip("How quickly you can duck and sidestep when an attack is blocked")]
    [SerializeField] private float blockedStickSpeed;

    [Tooltip("Lower stickSpeed to smooth out transitions to idle (when stick is centered)")]
    [SerializeField] private float smoothStickSpeed;

    protected override void Start()
    {
        target = GameObject.FindWithTag("Enemy").transform;
        base.Start();
    }

    override protected void Update()
    {
        // The player can only skip if they are blocking but they aren't attacking nor skipping already.
        isDashing = anim.GetCurrentAnimatorStateInfo(0).IsName("Dash");
        anim.SetBool("can_dash", !isAttacking && !isDashing && !isHurt);
        otherTracking = !isDashing || anim.IsInTransition(0);

        // Establish a direction towards which to dash that doesn't change while dashing.
        if (!isDashing) {
            anim.SetFloat("horizontal_dash", directionTarget.x);
            anim.SetFloat("vertical_dash", directionTarget.y);
        }

        // Change movement animation blending speed depending on the situation.
        if (directionTarget.magnitude == 0f && !block_pressed)
            directionSpeed = smoothStickSpeed;
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Block"))
            directionSpeed = blockingStickSpeed;
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Blocked"))
            directionSpeed = blockedStickSpeed;
        else if (isHurt)
            directionSpeed = 0f;
        else
            directionSpeed = stickSpeed;

        base.Update();
    }

    #region Input
    // Meant for Unity Input System events

    public void Movement(InputAction.CallbackContext context) { directionTarget = context.ReadValue<Vector2>().normalized; }
    public void Dash(InputAction.CallbackContext context) { anim.SetBool("dash", context.performed && block_pressed); }
    public void Sprint(InputAction.CallbackContext context) { anim.SetBool("sprint", context.performed); }
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
    public void Block(InputAction.CallbackContext context) { block_pressed = context.performed; }
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
    public bool isPlayerBlocking { get => block_pressed; }
    #endregion
}
