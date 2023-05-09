using Cinemachine;

public interface ICameraInitialize
{
    public void Initialize(ref CinemachineTargetGroup targetGroup, ref CinemachineVirtualCamera vcam, ref Player player, ref Enemy enemy);

    public void InitializeTarget(ref CameraTargets playerTargets, ref CameraTargets enemyTargets);
}
