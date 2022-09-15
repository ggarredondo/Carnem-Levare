using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //***VARIABLES***
    [Header("Movement Utitlities (Speed)")]

    public float speed = 1;
    public float cameraSpeed = 1;
    public float cameraRotationSpeed = 1;
    public float cameraBouncingSpeed = 1;
    public int cameraBouncingSteps = 1;

    [Header("Movement Utitlities (Movement)")]
    [Range(2f, 10.0f)]  public float widthApertureMovement;
    [Range(3f, 10.0f)] public float heightApertureMovement;
    [Range(0.1f, 1f)] public float cameraMovementDetectzone;
    [Range(1f, 20f)] public float playerMovementDetectzone;

    [Header("Movement Utitlities (Gameobjects)")]
    public Camera playerCamera;
    public GameObject boxingRing;

    [Header("Movement Utilities (Animation)")]
    public float punchAnimationPause; 
        
    private Transform cameraTransform;
    private Transform playerTransform;
    private Vector3 initialPlayerPosition;
    private Vector3 initialCameraPosition;
    private Rigidbody rb;
    private Animator playerAnim;

    private Vector2 widthResolutionLimits, heightResolutionLimits;

    private int counter;
    private int sign;
    private int actualbouncingSteps;

    private Vector2 directionMovement;
    private Vector2 cameraMovement;

    //***START_AND_UPDATE***

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cameraTransform = playerCamera.GetComponent<Transform>();
        playerTransform = gameObject.GetComponent<Transform>();
        playerAnim = gameObject.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        initialPlayerPosition = playerTransform.position;
        initialCameraPosition = cameraTransform.position;

        directionMovement = new Vector2(0, 0);

        widthResolutionLimits = new Vector2(Screen.width / widthApertureMovement, Screen.width - Screen.width / widthApertureMovement);
        heightResolutionLimits = new Vector2(Screen.height / heightApertureMovement, initialPlayerPosition.y);

        counter = 0;
        sign = 1;
        actualbouncingSteps = cameraBouncingSteps;
    }

    void Update()
    {
        PlayerMovement();
        PlayerComeBack();
        CameraMovement();
    }

    //***CONTROLS***

    /// <summary>
    /// Obtain the direction movement from the left stick
    /// </summary>
    public void MoveControl(InputAction.CallbackContext context)
    {
        //Obtain de direction from the left stick
        directionMovement = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Obtain the direction movement from the right stick
    /// </summary>
    public void CameraControl(InputAction.CallbackContext context)
    {
        //Obtain de direction from the left stick
        cameraMovement = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Obtain the right jab action
    /// </summary>
    public void RightJab(InputAction.CallbackContext context)
    {
        if (playerAnim.GetInteger("punchTypes") == 0 && context.started)
        {
            playerAnim.SetInteger("punchTypes", 1);
            Invoke("WaitAnimation", playerAnim.GetCurrentAnimatorStateInfo(0).length + punchAnimationPause);
        }
    }

    /// <summary>
    /// Obtain the left jab action
    /// </summary>
    public void LeftJab(InputAction.CallbackContext context)
    {
        if (playerAnim.GetInteger("punchTypes") == 0 && context.started)
        {
            playerAnim.SetInteger("punchTypes", 2);
            Invoke("WaitAnimation", playerAnim.GetCurrentAnimatorStateInfo(0).length + punchAnimationPause);
        }
    }

    /// <summary>
    /// Obtain the right punch action
    /// </summary>
    public void RightSuperPunch(InputAction.CallbackContext context)
    {
        if (playerAnim.GetInteger("punchTypes") == 0 && context.started)
        {
            playerAnim.SetInteger("punchTypes", 3);
            Invoke("WaitAnimation", playerAnim.GetCurrentAnimatorStateInfo(0).length + punchAnimationPause);
        }
    }

    /// <summary>
    /// Obtain the right left action
    /// </summary>
    public void LeftSuperPunch(InputAction.CallbackContext context)
    {
        if (playerAnim.GetInteger("punchTypes") == 0 && context.started)
        {
            playerAnim.SetInteger("punchTypes", 4);
            Invoke("WaitAnimation", playerAnim.GetCurrentAnimatorStateInfo(0).length + punchAnimationPause);
        }
    }

    //***MOVEMENT_FUNCTIONS***

    private void CameraMovement()
    {
        //Player movement around the ring
        if (playerAnim.GetInteger("punchTypes") == 0 && cameraMovement.x != 0)
        {
            //Camera movement around the ring
            cameraTransform.RotateAround(boxingRing.transform.position, Vector3.up, cameraMovement.x * cameraRotationSpeed * Time.deltaTime);

            //Rotate the initial camera point
            initialCameraPosition = RotateAroundPoint(initialCameraPosition, boxingRing.transform.position, Quaternion.Euler(0, cameraMovement.x * cameraRotationSpeed * Time.deltaTime, 0));
        }
    }

    private void PlayerMovement()
    {

        //Player movement around the ring
        if (playerAnim.GetInteger("punchTypes") == 0)
        {
            //Move the player
            rb.velocity = (Vector3) directionMovement * speed;


            //Limit the player movement, using the actual resolution to adecuate the movement for all monitors
            playerTransform.position = new Vector3(Mathf.Clamp(playerTransform.position.x, widthResolutionLimits.x, widthResolutionLimits.y),
                                                   Mathf.Clamp(playerTransform.position.y, heightResolutionLimits.x, heightResolutionLimits.y), 
                                                   playerTransform.position.z);

            //Camera movement forward and backward throught the ring
            if (playerTransform.position.y > heightResolutionLimits.x && playerTransform.position.y < initialPlayerPosition.y)
                cameraTransform.Translate(0, 0, directionMovement.y * cameraSpeed * Time.deltaTime);
            
            //Bouncing camera (walking effect)
            if (counter == actualbouncingSteps)
            {
                sign *= -1;
                counter = 0;
                actualbouncingSteps = Random.Range(cameraBouncingSteps-10, cameraBouncingSteps+10);
            }
            
            
            if (directionMovement != new Vector2(0, 0))
            {
                cameraTransform.Translate(0, sign * cameraBouncingSpeed * Time.deltaTime, 0);
                counter++;
                playerAnim.SetBool("moving", true);
            }
            else playerAnim.SetBool("moving", false);

        }
        else
        {
            //If player is hitting, player doesn't move
            rb.velocity = new Vector2(0, 0);
        }
    }

    /// <summary>
    /// The player and camera come back to the initial position after user move it
    /// </summary>
    private void PlayerComeBack()
    {
        //Move the player to the initial point if the player is not moving
        //There must be a detectzone because we cant reach the initial point exactly
        if (NearVectors(playerTransform.position, initialPlayerPosition, playerMovementDetectzone) && directionMovement == new Vector2(0, 0))
        {
            Vector3 playerMoveDir = (initialPlayerPosition - playerTransform.position).normalized;
            rb.velocity = playerMoveDir * speed;
        }

        //Move the camera to the initial point if the player is not moving
        if (NearVectors(cameraTransform.position, initialCameraPosition, cameraMovementDetectzone) && directionMovement == new Vector2(0, 0))
            cameraTransform.Translate(Vector3.forward * cameraSpeed * Time.deltaTime);
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

    /// <summary>
    /// Rotate a vector around a point
    /// </summary>
    /// <param name="point">The vector point we want to rotate</param>
    /// <param name="pivot">The points around we want to rotate de vector</param>
    /// <param name="angle">The angle of rotation</param>
    /// <returns></returns>
    private Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
    {
        return angle* (point - pivot) + pivot;
    }

    /// <summary>
    /// Wait until the punch animation ends
    /// </summary>
    private void WaitAnimation()
    {
        playerAnim.SetInteger("punchTypes", 0);
    }
}
