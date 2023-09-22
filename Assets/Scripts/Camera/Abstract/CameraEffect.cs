using Cinemachine;
using UnityEngine;

public abstract class CameraEffect : ScriptableObject
{
    protected CinemachineVirtualCamera vcam;

    protected bool applyCondition;

    [Header("Parameters")]

    public Tuple<float> speed;
    public float responseTime;

    public abstract void Initialize(ref CinemachineVirtualCamera vcam);

    public abstract void UpdateCondition(ref Player player, ref Enemy enemy);

    protected virtual void UpdateParameters(){ }

    public virtual void ReturnInitialPosition() { }

    public virtual void UpdateInitialPosition() { }

    public bool ApplyCondition { get { return applyCondition; } }
}