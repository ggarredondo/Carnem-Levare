using Cinemachine;

public interface ITargeting
{
    public void Initialize(ref CinemachineTargetGroup targetGroup, ref CinemachineVirtualCamera vcam, ref Player player, ref Enemy enemy);

    public void InitializeTarget(ref CameraTargets playerTargets, ref CameraTargets enemyTargets);
}
