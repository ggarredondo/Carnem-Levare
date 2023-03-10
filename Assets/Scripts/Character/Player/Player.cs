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

    public void Movement(InputAction.CallbackContext context) { base.Movement(context.ReadValue<Vector2>().normalized); }
    public void Dash(InputAction.CallbackContext context) { base.Dash(context.performed); }
    public void Block(InputAction.CallbackContext context) { base.Block(context.performed); }
    
    public void LeftNormal(InputAction.CallbackContext context) { base.LeftN(context.performed, 0); }
    public void LeftSpecial(InputAction.CallbackContext context) { base.LeftN(context.performed, 1); }

    /// <summary>
    /// To state if a move from right moveset is being pressed. Necessary for
    /// charging attacks.
    /// </summary>
    /// <param name="i">Move index in right moveset list</param>
    /// <param name="b">Press value</param>
    public void PressMove(int i, bool b) { RightMoveset[i].pressed = b; }
    public void RightNormal(InputAction.CallbackContext context) {
        PressMove(0, context.performed);
        base.RightN(context.performed, 0);
    }
    public void RightSpecial(InputAction.CallbackContext context) {
        PressMove(1, context.performed);
        base.RightN(context.performed, 1);
    }

    #endregion
}
