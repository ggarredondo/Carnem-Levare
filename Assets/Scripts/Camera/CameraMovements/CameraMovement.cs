using Cinemachine;
using UnityEngine;

public abstract class CameraMovement : ScriptableObject
{
    protected CinemachineVirtualCamera vcam;

    protected bool applyCondition;

    [Header("Management")]

    [SerializeField] protected TypeCameraMovement id;

    [SerializeField] protected bool stackable;

    [Header("Parameters")]

    public Tuple<float> aceleration;

    public abstract void Initialize(ref CinemachineVirtualCamera vcam);

    protected virtual void UpdateParameters(){ }

    public virtual void ReturnInitialPosition() { }

    public virtual void UpdateInitialPosition() { }

    public abstract void ApplyMove();

    public TypeCameraMovement ID { get { return id; } }

    public bool Stackable { get { return stackable; } }

    public bool ApplyCondition { get { return applyCondition; } set { applyCondition = value; } }
}

public enum TypeCameraMovement
{
    DOLLY_ZOOM,
    LINEAL_MOVE,
    NOISE,
    SMOOTH_FOLLOW
}