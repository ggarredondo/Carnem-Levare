using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Vector2 movement_value, direction;
    private float left_jab_value, right_jab_value, left_special_value, right_special_value, block_value;
    private bool is_attacking;

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
        Debug.Log(movement_value); // <----- DELETE
    }

    public void LeftJab(InputAction.CallbackContext context) { left_jab_value = context.ReadValue<float>(); }
    public void RightJab(InputAction.CallbackContext context) { right_jab_value = context.ReadValue<float>(); }
    public void LeftSpecial(InputAction.CallbackContext context) { left_special_value = context.ReadValue<float>(); }
    public void RightSpecial(InputAction.CallbackContext context) { right_special_value = context.ReadValue<float>(); }
    public void Block (InputAction.CallbackContext context) { block_value = context.ReadValue<float>();  }

    //***ANIMATION***

    /// <summary>
    /// Sets animation parameters for the animator.
    /// </summary>
    private void SetAnimationParameters()
    {
        is_attacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && !anim.IsInTransition(0);
        anim.SetBool("cant_attack", is_attacking || block_value > 0f);
        anim.SetFloat("load", load);
        anim.SetFloat("speed", speed);

        // MOVEMENT
        // Softens the movement by establishing the direction as a point that approaches the stick/mouse target.
        direction = Vector2.MoveTowards(direction, movement_value, movementSpeed * Time.deltaTime);
        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);
        transform.LookAt(new Vector3(TargetEnemy.position.x, transform.position.y, TargetEnemy.position.z)); // Rotate towards enemy.

        // ATTACKS
        anim.SetFloat("left_jab", left_jab_value);
        anim.SetFloat("right_jab", right_jab_value);
        anim.SetFloat("left_special", left_special_value);
        anim.SetFloat("right_special", right_special_value);

        // OTHER
        anim.SetFloat("block", block_value);
    }

    //***PUBLIC METHODS***

    public Animator getAnimator { get { return anim; } }

    public bool isWalking { get { return direction.x != 0f && !is_attacking; } }
}
