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
    [Range(0.1f, 1f)] public float cameraMovementDeadzone;
    [Range(1f, 10f)] public float playerMovementDeadzone;

    [Header("Movement Utitlities (Gameobjects)")]
    public Camera playerCamera;
    public GameObject boxingRing;
    
    private Transform cameraTransform;
    private Transform playerTransform;
    private Vector3 initialPlayerPosition;
    private Vector3 initialCameraPosition;
    private Rigidbody rb;
    private Animator playerAnim;

    private int widthResolution, heightResolution;

    private int counter;
    private int sign;

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

        widthResolution = Screen.currentResolution.width;
        heightResolution = Screen.currentResolution.height;

        counter = 0;
        sign = 1;
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
        //Actualizamos la posición de la cámara
        initialCameraPosition = cameraTransform.position;

        //Obtain de direction from the left stick
        cameraMovement = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 
    /// </summary>
    public void RightJab(InputAction.CallbackContext context)
    {
        if (playerAnim.GetInteger("punchTypes") == 0 && context.started)
        {
            playerAnim.SetInteger("punchTypes", 1);
            Invoke("WaitAnimation", playerAnim.GetCurrentAnimatorStateInfo(0).length + 0.2f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void LeftJab(InputAction.CallbackContext context)
    {
        if (playerAnim.GetInteger("punchTypes") == 0 && context.started)
        {
            playerAnim.SetInteger("punchTypes", 2);
            Invoke("WaitAnimation", playerAnim.GetCurrentAnimatorStateInfo(0).length + 0.2f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void RightSuperPunch(InputAction.CallbackContext context)
    {
        if (playerAnim.GetInteger("punchTypes") == 0 && context.started)
        {
            playerAnim.SetInteger("punchTypes", 3);
            Invoke("WaitAnimation", playerAnim.GetCurrentAnimatorStateInfo(0).length + 0.2f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void LeftSuperPunch(InputAction.CallbackContext context)
    {
        if (playerAnim.GetInteger("punchTypes") == 0 && context.started)
        {
            playerAnim.SetInteger("punchTypes", 4);
            Invoke("WaitAnimation", playerAnim.GetCurrentAnimatorStateInfo(0).length + 0.2f);
        }
    }

    //***MOVEMENT_FUNCTIONS***

    private void CameraMovement()
    {
        //Player movement around the ring
        if (playerAnim.GetInteger("punchTypes") == 0)
        {
            //Camera movement around the ring
            cameraTransform.RotateAround(boxingRing.transform.position, Vector3.up, cameraMovement.x * cameraRotationSpeed * Time.deltaTime);
        }
    }

    private void PlayerMovement()
    {

        //Player movement around the ring
        if (playerAnim.GetInteger("punchTypes") == 0)
        {
            //Move the player
            rb.velocity = (Vector3) directionMovement * speed;

            //Limit the player movement
            playerTransform.position = new Vector3(Mathf.Clamp(playerTransform.position.x, widthResolution / widthApertureMovement , widthResolution - widthResolution / widthApertureMovement),
                                                   Mathf.Clamp(playerTransform.position.y, heightResolution / heightApertureMovement, initialPlayerPosition.y), 
                                                   playerTransform.position.z);

            //Camera movement forward and backward throught the ring
            if(playerTransform.position.y > heightResolution / heightApertureMovement && playerTransform.position.y < initialPlayerPosition.y)
                cameraTransform.Translate(0, 0, directionMovement.y * cameraSpeed * Time.deltaTime);

            
            //Bouncing camera (walking effect)
            if (counter == 50)
            {
                sign *= -1;
                counter = 0;
            }
            
            if (directionMovement != new Vector2(0, 0))
            {
                cameraTransform.Translate(0, sign * cameraBouncingSpeed * Time.deltaTime, 0);
                playerAnim.SetBool("moving", true);
            }
            else playerAnim.SetBool("moving", false);

            counter++;

        }
        else
        {
            //If player is hitting, player doesn't move
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void PlayerComeBack()
    {

        if ((playerTransform.position.magnitude < initialPlayerPosition.magnitude - playerMovementDeadzone || playerTransform.position.magnitude > initialPlayerPosition.magnitude + playerMovementDeadzone) && 
            directionMovement == new Vector2(0, 0))
        {
            Vector3 playerMoveDir = (initialPlayerPosition - playerTransform.position).normalized;
            rb.velocity = playerMoveDir * speed;

            if (cameraTransform.position.magnitude > initialCameraPosition.magnitude - cameraMovementDeadzone || cameraTransform.position.magnitude > initialCameraPosition.magnitude + cameraMovementDeadzone)
                cameraTransform.Translate(Vector3.forward * cameraSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Wait until the punch animation ends
    /// </summary>
    private void WaitAnimation()
    {
        playerAnim.SetInteger("punchTypes", 0);
    }

}
