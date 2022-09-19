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
    private const float vertical_min = -1f, vertical_max = 0f;

    [Header("Animation Parameters")]
    [Range(-2f, 2f)] public float speed = 1f;
    [Range(-1f, 1f)] public float load = 0f;

    [Header("Movement Parameters")]
    [Range(0f, 10f)] public float movementSpeed = 1f;
    [Range(0f, 10f)] public float returnSpeed = 1f;

    [Range(0f, 10f)] public float translateSpeed = 1f;
    [Range(0f, 2f)] public float playerTranslateDetectzone;

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
        anim.SetFloat("vertical", vertical);
        anim.SetFloat("horizontal", horizontal);
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
            vertical += left_stick.y * Time.deltaTime * movementSpeed;

        else // If left stick isn't being pushed, character returns to center
            vertical += Time.deltaTime * returnSpeed;
        vertical = Mathf.Clamp(vertical, vertical_min, vertical_max);
    }

    /// <summary>
    /// Translates the player actor from left to right according to left stick
    /// </summary>
    private void PlayerTranslate()
    {
        Vector3 movement = new Vector3(left_stick.x  * translateSpeed, 0, 0);
        transform.Translate(movement * Time.deltaTime);
    }

    /// <summary>
    /// The player and camera come back to the initial position after user moves it
    /// </summary>
    private void PlayerTranslateComeback()
    {
        //Translates the player to the initial point if the player is not moving
        //There must be a detect zone because we can't reach the initial point exactly
        if (NearVectors(transform.position, initialPlayerPosition, playerTranslateDetectzone) && left_stick == new Vector2(0, 0))
        {
            Vector3 playerMoveDir = (transform.position - initialPlayerPosition) * translateSpeed;
            transform.Translate(playerMoveDir *  Time.deltaTime);
        }
    }

    /// <summary>
    /// Check if the distance between vectors is greater than a value
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
