using Cinemachine;
using UnityEngine;

public abstract class CameraMovement : ScriptableObject
{
    protected CinemachineVirtualCamera vcam;

    protected bool applyCondition;

    [Header("Parameters")]

    public Tuple<float> duration;

    public abstract void Initialize(ref CinemachineVirtualCamera vcam);

    public abstract void UpdateCondition(ref Player player);

    protected virtual void UpdateParameters(){ }

    public virtual void ReturnInitialPosition() { }

    public virtual void UpdateInitialPosition() { }

    public bool ApplyCondition { get { return applyCondition; } }
}