using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CameraType changeVirtualCamera;
    public static CameraType actualVirtualCamera;

    private CameraTargets playerTargets, enemyTargets;
    [SerializeField] private Transform actualTransform;
    [SerializeField] private GameObject followObject;

    private Player player;
    private Enemy enemy;

    private CinemachineTargetGroup targetGroup;
    private CinemachineVirtualCamera actualCinemachineCamera;

    private void Start()
    {
        playerTargets = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraTargets>();
        enemyTargets = GameObject.FindGameObjectWithTag("Enemy").GetComponent<CameraTargets>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        targetGroup = GameObject.FindGameObjectWithTag("TARGET_GROUP").GetComponent<CinemachineTargetGroup>();

        actualCinemachineCamera = GetComponentsInChildren<CinemachineVirtualCamera>()[0];

        InitializeTargets();
    }

    private void LateUpdate()
    {
        if (actualVirtualCamera != changeVirtualCamera) ChangeVirtualCamera();

        actualTransform.position = playerTargets.GetDefaultTarget(actualVirtualCamera).position;
        SetFollowObject();

        if (actualVirtualCamera == 0) actualTransform.LookAt(enemyTargets.GetDefaultTarget(actualVirtualCamera).position);
    }

    private void SetFollowObject()
    {
        followObject.transform.position = actualCinemachineCamera.transform.position;
        followObject.transform.rotation = actualCinemachineCamera.transform.rotation;
        followObject.transform.localPosition = actualCinemachineCamera.transform.localPosition;
        followObject.transform.localEulerAngles = actualCinemachineCamera.transform.localEulerAngles;
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

            if(camera.TryGetComponent(out ICameraInitialize targeting))
            {
                targeting.Initialize(ref targetGroup, ref tmp, ref player, ref enemy);
                targeting.InitializeTarget(ref playerTargets, ref enemyTargets);
            }
        }
    }

    private void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }

    private void ChangeVirtualCamera()
    {
        actualVirtualCamera = changeVirtualCamera;

        int cont = 0;
        foreach (CinemachineVirtualCamera camera in GetComponentsInChildren<CinemachineVirtualCamera>())
        {
            if (actualVirtualCamera == CameraType.GOPRO)
            {
                SetLayerAllChildren(followObject.transform, LayerMask.NameToLayer("NoCamera"));
            }
            else
            {
                SetLayerAllChildren(followObject.transform, LayerMask.NameToLayer("Default"));
            }

            if (cont == (int) actualVirtualCamera)
            {
                camera.enabled = true;
                actualCinemachineCamera = camera;

                if (camera.TryGetComponent(out CameraEffects effects))
                {
                    effects.enabled = true;
                }
            }
            else
            {
                camera.enabled = false;

                if (camera.TryGetComponent(out CameraEffects effects))
                {
                    effects.enabled = false;
                }
            }

            cont++;
        }
    }

}

public enum CameraType
{
    DEFAULT = 0,
    GOPRO = 1
}
