using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour, IObjectInitialize
{
    public CameraType changeVirtualCamera;
    public static CameraType currentVirtualCamera;

    private CameraTargets playerTargets, enemyTargets;

    [SerializeField] private Transform actualTransform;
    [SerializeField] private GameObject followObject;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    private Player player;
    private Enemy enemy;

    private CinemachineVirtualCamera currentCinemachineCamera;

    public void Initialize(ref GameObject player, ref GameObject enemy)
    {
        playerTargets = player.GetComponent<CameraTargets>();
        enemyTargets = enemy.GetComponent<CameraTargets>();

        this.player = player.GetComponent<Player>();
        this.enemy = enemy.GetComponent<Enemy>();

        currentCinemachineCamera = GetComponentsInChildren<CinemachineVirtualCamera>()[0];

        InitializeTargets();
    }

    private void LateUpdate()
    {
        if (currentVirtualCamera != changeVirtualCamera) ChangeVirtualCamera();

        actualTransform.position = playerTargets.GetDefaultTarget(currentVirtualCamera).position;
        SetFollowObject();

        if (currentVirtualCamera == 0) actualTransform.LookAt(enemyTargets.GetDefaultTarget(currentVirtualCamera).position);
    }

    private void SetFollowObject()
    {
        followObject.transform.position = currentCinemachineCamera.transform.position;
        followObject.transform.rotation = currentCinemachineCamera.transform.rotation;
        followObject.transform.localPosition = currentCinemachineCamera.transform.localPosition;
        followObject.transform.localEulerAngles = currentCinemachineCamera.transform.localEulerAngles;
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
                effects.Initialize(ref tmp, ref player, ref enemy);
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
        currentVirtualCamera = changeVirtualCamera;

        int cont = 0;
        foreach (CinemachineVirtualCamera camera in GetComponentsInChildren<CinemachineVirtualCamera>())
        {
            if (currentVirtualCamera == CameraType.GOPRO)
            {
                SetLayerAllChildren(followObject.transform, LayerMask.NameToLayer("NoCamera"));
            }
            else
            {
                SetLayerAllChildren(followObject.transform, LayerMask.NameToLayer("Default"));
            }

            if (cont == (int) currentVirtualCamera)
            {
                camera.enabled = true;
                currentCinemachineCamera = camera;

                if (camera.TryGetComponent(out CameraEffects effects))
                {
                    effects.enabled = true;
                }

                if (camera.TryGetComponent(out Volume postProcess ))
                {
                    postProcess.enabled = true;
                }
            }
            else
            {
                camera.enabled = false;

                if (camera.TryGetComponent(out CameraEffects effects))
                {
                    effects.enabled = false;
                }

                if (camera.TryGetComponent(out Volume postProcess))
                {
                    postProcess.enabled = false;
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
