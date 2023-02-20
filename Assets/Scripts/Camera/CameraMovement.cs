using Cinemachine;
using UnityEngine;

public abstract class CameraMovement : MonoBehaviour
{
    protected CinemachineVirtualCamera vcam;

    public Tuple<float> aceleration;

    private void Awake()
    {
        Initialize();
    }

    public abstract void Initialize();

    public abstract void ApplyMove(bool condition);
}