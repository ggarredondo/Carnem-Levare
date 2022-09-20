using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;

    private Vector2 left_stick;
    private Vector3 initialPlayerPosition;

    private float horizontal = 0f, vertical = 0f;
    private const float vertical_min = -1f, vertical_max = 0f, horizontal_min = -1f, horizontal_max = 1f;

    [Header("Animation Parameters")]
    [Range(-2f, 2f)] public float speed = 1f;
    [Range(0f, 1f)] public float load = 0f;

    [Header("Movement Parameters")]
    [Range(0f, 10f)] public float movementSpeed = 1f;
    [Range(0f, 10f)] public float returnSpeed = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        initialPlayerPosition = transform.position;
    }

    void Update()
    {
        Animation();
        PlayerMovement();
    }

    /// <summary>
    /// Set animation parameters for the animator
    /// </summary>
    private void Animation()
    {
        anim.SetFloat("load", load);
        anim.SetFloat("speed", speed);
        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("vertical", vertical);
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
        // If left stick is pushed, character moves according to the stick
        if (left_stick.magnitude > 0f)
        {
            horizontal += left_stick.x * Time.deltaTime * movementSpeed;
            vertical += left_stick.y * Time.deltaTime * movementSpeed;
        }

        else // If left stick isn't being pushed, character returns to center
        {
            horizontal += left_stick.x * Time.deltaTime * returnSpeed;
            vertical += Time.deltaTime * returnSpeed;
        }

        horizontal = Mathf.Clamp(horizontal, horizontal_min, horizontal_max);
        vertical = Mathf.Clamp(vertical, vertical_min, vertical_max);
    }
}
