using Cinemachine;
using UnityEngine;

public abstract class CameraMovement : MonoBehaviour
{
    protected CinemachineVirtualCamera vcam;

    public CameraMovement(CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
    }

    public Tuple<float> aceleration;

    public abstract void ApplyMove(bool condition);
}