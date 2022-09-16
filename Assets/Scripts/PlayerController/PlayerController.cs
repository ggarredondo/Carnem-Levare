using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    private Vector2 left_stick;

    private Vector3 initialPlayerPosition;

    [Header("Animation Parameters")]
    [Range(-1f, 1f)] public float load = 0f;
    [Range(-2f, 2f)] public float speed = 1f;

    [Header("Movement Parameters")]
    [Range(0f, 10f)] public float movementSpeed = 0f;
    [Range(0f, 2f)] public float playerMovementDetectzone;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        initialPlayerPosition = transform.position;
    }

    void Update()
    {
        Animation();
        PlayerMovement();
        PlayerComeBack();
    }

    /// <summary>
    /// Set animation parameters for the animator
    /// </summary>
    private void Animation()
    {
        anim.SetFloat("load", load);
        anim.SetFloat("speed", speed);
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
    /// Make the player move with the left stick values
    /// </summary>
    private void PlayerMovement()
    {
        Vector3 movement = new Vector3(left_stick.x  * movementSpeed, 0, 0);
        transform.Translate(movement * Time.deltaTime);
    }

    /// <summary>
    /// The player and camera come back to the initial position after user move it
    /// </summary>
    private void PlayerComeBack()
    {
        //Move the player to the initial point if the player is not moving
        //There must be a detectzone because we cant reach the initial point exactly
        if (NearVectors(transform.position, initialPlayerPosition, playerMovementDetectzone) && left_stick == new Vector2(0, 0))
        {
            Vector3 playerMoveDir = (transform.position - initialPlayerPosition) * movementSpeed;
            transform.Translate(playerMoveDir *  Time.deltaTime);
        }
    }

    /// <summary>
    /// Check if the distance between vectors is greather than a value
    /// </summary>
    /// <param name="actualPosition">The actual point</param>
    /// <param name="target">The target point that defines the distance</param>
    /// <param name="detectzone">The value to be greather than</param>
    /// <returns></returns>
    private bool NearVectors(Vector3 actualPosition, Vector3 target, float detectzone)
    {
        return Vector3.Distance(actualPosition, target) > detectzone;
    }
}
