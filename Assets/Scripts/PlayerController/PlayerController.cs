using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Vector2 movement_value, direction;
    private float last_left_special_value, left_special_value, last_right_special_value, right_special_value;
    private bool is_attacking, pressed_block, pressed_left_jab, pressed_right_jab, pressed_left_dodge, pressed_right_dodge;

    public Transform TargetEnemy;

    [Header("Animation Parameters")]
    public float speed = 1f;
    [Range(0f, 1f)] public float load = 0f;

    [Header("Movement Parameters")]
    public float movementSpeed = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        movement_value = Vector2.zero;
        direction = Vector2.zero;
    }

    void Update()
    {
        SetAnimationParameters();
    }

    //***INPUT***

    public void Movement(InputAction.CallbackContext context)
    { 
        movement_value = context.ReadValue<Vector2>();
        movement_value.y = Mathf.Clamp(movement_value.y, -1f, 0f); // -1 is crouching, 0 is standing. Doesn't make sense to consider 1 as a value.
    }

    public void LeftJab(InputAction.CallbackContext context) { pressed_left_jab = context.ReadValue<float>() > 0f; }
    public void RightJab(InputAction.CallbackContext context) { pressed_right_jab = context.ReadValue<float>() > 0f; }
    public void LeftSpecial(InputAction.CallbackContext context) { left_special_value = context.ReadValue<float>(); }
    public void RightSpecial(InputAction.CallbackContext context) { right_special_value = context.ReadValue<float>(); }
    public void Block (InputAction.CallbackContext context) { pressed_block = context.ReadValue<float>() > 0f; }
    public void LeftDodge(InputAction.CallbackContext context) { pressed_left_dodge = context.performed; }
    public void RightDodge(InputAction.CallbackContext context) { pressed_right_dodge = context.performed; }

    //***ANIMATION***

    /// <summary>
    /// Sets animation parameters for the animator.
    /// </summary>
    private void SetAnimationParameters()
    {
        is_attacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && !anim.IsInTransition(0);
        anim.SetBool("cant_attack", is_attacking || pressed_block);
        anim.SetFloat("load", load);
        anim.SetFloat("speed", speed);

        // MOVEMENT
        // Softens the movement by establishing the direction as a point that approaches the stick/mouse target.
        direction = Vector2.MoveTowards(direction, movement_value, movementSpeed * Time.deltaTime);
        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);
        transform.LookAt(new Vector3(TargetEnemy.position.x, transform.position.y, TargetEnemy.position.z)); // Rotate towards enemy.

        // ATTACKS
        anim.SetBool("left_jab", pressed_left_jab);
        anim.SetBool("right_jab", pressed_right_jab);

        //if (last_left_special_value > 0f) left_special_value = 0f; // Ensure you can't spam special punches by holding.
        anim.SetFloat("left_special", left_special_value);
        //last_left_special_value = left_special_value;

        //if(last_right_special_value > 0f) right_special_value = 0f;
        anim.SetFloat("right_special", right_special_value);
        //last_right_special_value = right_special_value;

        // OTHER
        anim.SetBool("block", pressed_block);
    }

    //***PUBLIC METHODS***

    public Animator getAnimator { get { return anim; } }

    public bool isWalking { get { return direction.x != 0f && !is_attacking; } }
}
