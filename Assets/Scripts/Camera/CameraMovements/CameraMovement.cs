using Cinemachine;
using UnityEngine;

public abstract class CameraMovement : ScriptableObject
{
    protected CinemachineVirtualCamera vcam;

    [Header("Management")]

    [SerializeField] protected TypeCameraMovement id;

    [SerializeField] protected bool stackable;

    [Header("Parameters")]

    public Tuple<float> aceleration;

    public abstract void Initialize(CinemachineVirtualCamera vcam);

    protected virtual void UpdateParameters(){ }

    public virtual void ReturnInitialPosition() { }

    public virtual void UpdateInitialPosition() { }

    public abstract void ApplyMove(bool condition);

    public TypeCameraMovement ID { get { return id; } }

    public bool Stackable { get { return stackable; } }
}

public enum TypeCameraMovement
{
    DOLLY_ZOOM,
    LINEAL_MOVE,
    NOISE,
    SMOOTH_FOLLOW
}