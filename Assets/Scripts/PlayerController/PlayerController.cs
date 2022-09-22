using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Vector2 left_stick;

    [Header("Animation Parameters")]
    [Range(-2f, 2f)] public float speed = 1f;
    [Range(0f, 1f)] public float load = 0f;

    [Header("Movement Parameters")]
    [Range(0f, 10f)] public float movementSpeed = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Animation();
        Debug.Log("Left stick magnitude: " + left_stick.magnitude);
    }

    /// <summary>
    /// Set animation parameters for the animator
    /// </summary>
    private void Animation()
    {
        anim.SetFloat("load", load);
        anim.SetFloat("speed", speed);
        anim.SetFloat("horizontal", left_stick.x);
        anim.SetFloat("vertical", left_stick.y);
        anim.SetBool("is_moving", left_stick.magnitude > 0f);
    }

    //***CONTROLS***

    /// <summary>
    /// Obtain the direction movement from the left stick
    /// </summary>
    public void MoveControl(InputAction.CallbackContext context)
    {
        left_stick = context.ReadValue<Vector2>().normalized;
    }

    //***MOVEMENT_FUNCTIONS***

    /// <summary>
    /// Modifies animator parameters to move the player through animations
    /// </summary>
    private void PlayerMovement()
    {
        // nothing yet
    }
}
