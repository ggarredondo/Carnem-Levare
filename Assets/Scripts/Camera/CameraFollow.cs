

using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{

    //Max value of the damping parameter (Virtual camera)
    const float CAMERA_SPEED_DIVIDER = 1000;
    const float INITIAL_REDUCE_DAMPING = 1;

    [Header("Target Objects")]
    public PlayerController playerController;
    public CinemachineVirtualCamera vcam;

    [Header("Camera Parameters")]
    [Range(0f, 1f)] public float cameraAceleration;
    public float MAX_DAMPING = 20;
    public float MIN_DAMPING = 0;
    public bool cameraRotation;

    private float damping;
    private CinemachineTransposer transposer;
    private float reduceDamping;

    //Temporal variable while we are testing decrementReduce on inspector
    private float cameraAcelerationTmp;

    private void Awake()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Start()
    {
        reduceDamping = INITIAL_REDUCE_DAMPING;
    }

    private void LateUpdate()
    {
        if (Time.timeScale == 1)
        {
            //Apply the inspector parameter with the real value we need
            cameraAcelerationTmp = cameraAceleration / CAMERA_SPEED_DIVIDER;
            CameraMovement();
        }
    }

    /// <summary>
    /// The camera always looks at the player but stops following him when its moving.
    /// </summary>
    private void CameraMovement()
    {
        if (cameraRotation)
        {
            if (playerController.isWalking)
            {
                damping = MAX_DAMPING;
                transposer.m_YawDamping = damping;
                reduceDamping = INITIAL_REDUCE_DAMPING;
            }
            else
            {
                damping *= reduceDamping;

                //Decrement the reduceDamping to make feel aceleration
                reduceDamping -= cameraAcelerationTmp;

                //Make values be under damping parameter range
                damping = Mathf.Clamp(damping, MIN_DAMPING, MAX_DAMPING);
                reduceDamping = Mathf.Clamp(reduceDamping, MIN_DAMPING, MAX_DAMPING);

                //Apply to the virtual camera parameter
                transposer.m_YawDamping = damping;

            }
        }
    }

}