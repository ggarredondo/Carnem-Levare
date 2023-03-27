using Cinemachine;
using UnityEngine;

public abstract class CameraMovement : ScriptableObject
{
    protected CinemachineVirtualCamera vcam;

    [Header("Management")]

    [SerializeField] protected TypeCameraMovement id;

    [SerializeField] protected bool stackable;

    [ConditionalField("stackable")] [SerializeField] protected bool cancelable;

    [Header("Parameters")]

    public Tuple<float> aceleration;

    public abstract void Initialize(CinemachineVirtualCamera vcam);

    public virtual void UpdateParameters(){ }

    public virtual bool InitialPosition() { return false; }

    public abstract void ApplyMove(bool condition);

    public TypeCameraMovement ID { get { return id; } }

    public bool Stackable { get { return stackable; } }

    public bool Cancelable { get { return cancelable; } }
}

public enum TypeCameraMovement
{
    DOLLY_ZOOM,
    LINEAL_MOVE,
    NOISE,
    SMOOTH_FOLLOW
}