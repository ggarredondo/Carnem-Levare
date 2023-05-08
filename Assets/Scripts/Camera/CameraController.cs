using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CameraType changeVirtualCamera;
    public static CameraType actualVirtualCamera;

    private CameraTargets playerTargets, enemyTargets;
    [SerializeField] private Transform actualTransform;

    private Player player;
    private Enemy enemy;

    private CinemachineTargetGroup targetGroup;

    private void Start()
    {
        playerTargets = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraTargets>();
        enemyTargets = GameObject.FindGameObjectWithTag("Enemy").GetComponent<CameraTargets>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        targetGroup = GameObject.FindGameObjectWithTag("TARGET_GROUP").GetComponent<CinemachineTargetGroup>();

        InitializeTargets();
    }

    private void LateUpdate()
    {
        if (actualVirtualCamera != changeVirtualCamera) ChangeVirtualCamera();

        actualTransform.position = playerTargets.GetDefaultTarget(actualVirtualCamera).position;
        if(actualVirtualCamera == 0) actualTransform.LookAt(enemyTargets.GetDefaultTarget(actualVirtualCamera).position);
    }

    private void InitializeTargets()
    {
        targetGroup.m_Targets[0].target = playerTargets.GetDefaultTarget(CameraType.DEFAULT);
        targetGroup.m_Targets[1].target = enemyTargets.GetDefaultTarget(CameraType.DEFAULT);

        foreach (CinemachineVirtualCamera camera in GetComponentsInChildren<CinemachineVirtualCamera>())
        {
            camera.m_Follow = actualTransform;

            CinemachineVirtualCamera tmp = camera;

            if (camera.TryGetComponent(out CameraEffects effects)) 
            {
                effects.Initialize(ref tmp, ref player);
            }

            if(camera.TryGetComponent(out ITargeting targeting))
            {
                targeting.Initialize(ref targetGroup, ref tmp, ref player, ref enemy);
                targeting.InitializeTarget(ref playerTargets, ref enemyTargets);
            }
        }
    }

    private void ChangeVirtualCamera()
    {
        actualVirtualCamera = changeVirtualCamera;

        int cont = 0;
        foreach (CinemachineVirtualCamera camera in GetComponentsInChildren<CinemachineVirtualCamera>())
        {
            if (cont == (int) actualVirtualCamera)
            {
                camera.enabled = true;
                camera.GetComponent<CameraEffects>().enabled = true;
            }
            else
            {
                camera.enabled = false;
                camera.GetComponent<CameraEffects>().enabled = false;
            }

            cont++;
        }
    }

}

public enum CameraType
{
    DEFAULT = 0,
    FIRST_PERSON = 1,
    GOPRO = 2,
    DRONE = 3
}
