using Cinemachine;
using UnityEngine;

public class GoProCamera : MonoBehaviour, ITargeting
{
    private CinemachineVirtualCamera vcam;

    public void Initialize(ref CinemachineTargetGroup targetGroup, ref CinemachineVirtualCamera vcam, ref Player player, ref Enemy enemy)
    {
        this.vcam = vcam;
    }

    public void InitializeTarget(ref CameraTargets playerTargets, ref CameraTargets enemyTargets)
    {
        vcam.m_LookAt = playerTargets.GetAlternativeTarget(CameraType.GOPRO);
    }
}
