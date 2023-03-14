using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : Character
{
    [Header("Enemy-specific Parameters")]
    [SerializeField] private float enemyDirectionSpeed = 1f;
    [SerializeField] private bool block;

    protected override void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        base.Start();
    }

    protected override void Update()
    {
        directionSpeed = enemyDirectionSpeed;
        base.Block(block); // DebugInput
        base.Update();
    }

    #region DebugInput
    public void Movement(InputAction.CallbackContext context) { base.Movement(-context.ReadValue<Vector2>().normalized); }
    public void Block(InputAction.CallbackContext context) { block = context.performed ? !block : block; }

    public void Left0(InputAction.CallbackContext context) { base.LeftN(context.performed, 0); }
    public void Left1(InputAction.CallbackContext context) { base.LeftN(context.performed, 1); }
    public void Left2(InputAction.CallbackContext context) { base.LeftN(context.performed, 2); }

    public void Right0(InputAction.CallbackContext context) { base.RightN(context.performed, 0); }
    public void Right1(InputAction.CallbackContext context) { base.RightN(context.performed, 1); }
    public void Right2(InputAction.CallbackContext context) { base.RightN(context.performed, 2); }
    #endregion
}
