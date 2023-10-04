using Cinemachine;
using UnityEngine;

public abstract class CameraEffect : MonoBehaviour
{
    protected CinemachineVirtualCamera vcam;
    protected bool applyCondition;

    public abstract void Initialize(ref CinemachineVirtualCamera vcam);

    public abstract void UpdateCondition(ref Player player, ref Enemy enemy);

    protected virtual void UpdateParameters(){ }

    public virtual void ReturnInitialPosition() { }

    public virtual void UpdateInitialPosition() { }

    public bool ApplyCondition { get { return applyCondition; } }
}