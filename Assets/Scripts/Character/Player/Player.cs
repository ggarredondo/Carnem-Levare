using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
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
        // Change movement animation blending speed depending on the situation.
        if (directionTarget.magnitude == 0f && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block"))
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
    public void Dash(InputAction.CallbackContext context) { anim.SetBool("dash", context.started); }
    public void Block(InputAction.CallbackContext context) { anim.SetBool("block", context.performed); }
    
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
    
    #endregion
}
