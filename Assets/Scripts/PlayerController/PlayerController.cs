using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Vector2 movementValue, direction;
    private bool isAttacking, isBlocking, cantAttack;
    private bool tappedLeftNormal, tappedRightNormal, tappedLeftSpecial, tappedRightSpecial;

    public Transform TargetEnemy;

    [Header("Animation Parameters")]
    public float generalSpeed = 1f;
    [Range(0f, 1f)] public float load = 0f;
    private float leftJabSpeed = 1f, rightJabSpeed = 1f, leftSpecialSpeed = 1f, rightSpecialSpeed = 1f, dodgeSpeed = 1f;

    [Header("Movement Parameters")]
    public float movementSpeed = 8f;
    [Range(0f, 1f)] public float attackingModifier = 0f, blockingModifier = 0f; // The player may move slower when attacking or blocking.
    private float current_movementSpeed;
    [Range(-1f, 0f)] public float duckingRange = -1f;

    [Header("Attack Parameters")]
    public Move leftNormalSlot;
    public Move rightNormalSlot;
    public Move leftSpecialSlot;
    public Move rightSpecialSlot;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        movementValue = Vector2.zero;
        direction = Vector2.zero;
        isAttacking = false;
        isBlocking = false;
        cantAttack = false;
    }

    void Update()
    {
        SetAnimationParameters();
    }

    //***INPUT***

    public void Movement(InputAction.CallbackContext context)
    { 
        movementValue = context.ReadValue<Vector2>();
        movementValue.y = Mathf.Clamp(movementValue.y, duckingRange, 0f); // -1 is crouching, 0 is standing. Doesn't make sense to consider 1 as a value.
    }

    public void LeftJab(InputAction.CallbackContext context) { tappedLeftNormal = context.performed; }
    public void RightJab(InputAction.CallbackContext context) { tappedRightNormal = context.performed; }
    public void LeftSpecial(InputAction.CallbackContext context) { tappedLeftSpecial = context.performed; }
    public void RightSpecial(InputAction.CallbackContext context) { tappedRightSpecial = context.performed; }
    public void Block (InputAction.CallbackContext context) { anim.SetBool("block", context.performed); }
    public void Dodge(InputAction.CallbackContext context) { anim.SetBool("dodge", context.performed); }

    //***ANIMATION***

    /// <summary>
    /// Sets general animation parameters for the animator.
    /// </summary>
    private void SetAnimationParameters()
    {
        // Values that must be updated frame by frame to allow certain animations to play out accordingly.
        isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && !anim.IsInTransition(0);
        isBlocking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Blocking") && !anim.IsInTransition(0);
        anim.SetBool("cant_attack", cantAttack);
        
        // Animation modifiers
        anim.SetFloat("load", load);
        anim.SetFloat("left_jab_speed", leftJabSpeed * generalSpeed);
        anim.SetFloat("right_jab_speed", rightJabSpeed * generalSpeed);
        anim.SetFloat("left_special_speed", leftSpecialSpeed * generalSpeed);
        anim.SetFloat("right_special_speed", rightSpecialSpeed * generalSpeed);
        anim.SetFloat("dodge_speed", dodgeSpeed * generalSpeed);

        // ATTACKS
        anim.SetBool("left_jab", tappedLeftNormal); tappedLeftNormal = false; // Must reset so that the player doesn't get stuck in a punching animation.
        anim.SetBool("right_jab", tappedRightNormal); tappedRightNormal = false;
        anim.SetBool("left_special", tappedLeftSpecial); tappedLeftSpecial = false;
        anim.SetBool("right_special", tappedRightSpecial); tappedRightSpecial = false;

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
