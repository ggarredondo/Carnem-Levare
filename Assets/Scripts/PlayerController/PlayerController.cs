using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Vector2 movement_value, direction;
    private bool is_attacking, is_blocking, is_dodging;
    private bool tapped_left_jab, tapped_right_jab, tapped_left_special, tapped_right_special;

    public Transform TargetEnemy;

    [Header("Animation Parameters")]
    public float generalSpeed = 1f;
    [Range(0f, 1f)] public float load = 0f;
    public float leftJabSpeed = 1f, rightJabSpeed = 1f, leftSpecialSpeed = 1f, rightSpecialSpeed = 1f, dodgeSpeed = 1f;

    [Header("Movement Parameters")]
    public float movementSpeed = 8f;
    [Range(0f, 1f)] public float attackingModifier = 0f, blockingModifier = 0f; // The player may move slower when attacking or blocking.
    private float current_movementSpeed;
    [Range(-1f, 0f)] public float duckingRange = -1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        movement_value = Vector2.zero;
        direction = Vector2.zero;
        is_attacking = false;
        is_blocking = false;
        is_dodging = false;
    }

    void Update()
    {
        SetAnimationParameters();
    }

    //***INPUT***

    public void Movement(InputAction.CallbackContext context)
    { 
        movement_value = context.ReadValue<Vector2>();
        movement_value.y = Mathf.Clamp(movement_value.y, duckingRange, 0f); // -1 is crouching, 0 is standing. Doesn't make sense to consider 1 as a value.
    }

    public void LeftJab(InputAction.CallbackContext context) { tapped_left_jab = context.performed; }
    public void RightJab(InputAction.CallbackContext context) { tapped_right_jab = context.performed; }
    public void LeftSpecial(InputAction.CallbackContext context) { tapped_left_special = context.performed; }
    public void RightSpecial(InputAction.CallbackContext context) { tapped_right_special = context.performed; }
    public void LeftSpecialStrong(InputAction.CallbackContext context) { /* Debug.Log(context.performed); */ }
    public void RightSpecialStrong(InputAction.CallbackContext context) { /* Debug.Log(context.performed); */ }
    public void Block (InputAction.CallbackContext context) { anim.SetBool("block", context.performed); }
    public void Dodge(InputAction.CallbackContext context) { anim.SetBool("dodge", context.performed); }

    //***ANIMATION***

    /// <summary>
    /// Sets general animation parameters for the animator.
    /// </summary>
    private void SetAnimationParameters()
    {
        // Values that must be updated frame by frame to allow certain animations to play out accordingly.
        is_attacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && !anim.IsInTransition(0);
        is_blocking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Blocking") && !anim.IsInTransition(0);
        is_dodging = anim.GetCurrentAnimatorStateInfo(0).IsTag("Dodging") && !anim.IsInTransition(0);
        anim.SetBool("is_attacking", is_attacking);
        anim.SetBool("is_dodging", is_dodging);
        anim.SetBool("cant_attack", is_attacking || is_blocking || is_dodging);
        
        // Animation modifiers
        anim.SetFloat("load", load);
        anim.SetFloat("left_jab_speed", leftJabSpeed * generalSpeed);
        anim.SetFloat("right_jab_speed", rightJabSpeed * generalSpeed);
        anim.SetFloat("left_special_speed", leftSpecialSpeed * generalSpeed);
        anim.SetFloat("right_special_speed", rightSpecialSpeed * generalSpeed);
        anim.SetFloat("dodge_speed", dodgeSpeed * generalSpeed);

        // ATTACKS
        anim.SetBool("left_jab", tapped_left_jab); tapped_left_jab = false; // Must reset so that the player doesn't get stuck in a punching animation.
        anim.SetBool("right_jab", tapped_right_jab); tapped_right_jab = false;
        anim.SetBool("left_special", tapped_left_special); tapped_left_special = false;
        anim.SetBool("right_special", tapped_right_special); tapped_right_special = false;

        // MOVEMENT
        // Softens the movement by establishing the direction as a point that approaches the stick/mouse position.
        current_movementSpeed = movementSpeed - movementSpeed * attackingModifier * System.Convert.ToSingle(is_attacking)
            - movementSpeed * blockingModifier * System.Convert.ToSingle(is_blocking);
        direction = Vector2.MoveTowards(direction, movement_value, current_movementSpeed * Time.deltaTime);
        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);
        transform.LookAt(new Vector3(TargetEnemy.position.x, transform.position.y, TargetEnemy.position.z)); // Rotate towards enemy.
    }

    //***PUBLIC METHODS***

    public Animator getAnimator { get { return anim; } }

    public bool isWalking { get { return direction.x != 0f && !is_attacking; } }
}
