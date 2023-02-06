using Cinemachine;
using UnityEngine;

public abstract class CameraMovement : MonoBehaviour
{
    protected CinemachineVirtualCamera vcam;

    public virtual void Awake()
    {
        vcam = GameObject.FindGameObjectWithTag("CAMERA").GetComponentInChildren<CinemachineVirtualCamera>();
    } 

    public Tuple<float> aceleration;

    public abstract void ApplyMove(bool condition);
}