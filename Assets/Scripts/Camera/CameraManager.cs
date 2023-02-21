using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public VirtualCameras changeVirtualCamera;
    public static VirtualCameras actualVirtualCamera;

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

    private void LateUpdate()
    {
        if (actualVirtualCamera != changeVirtualCamera) ChangeVirtualCamera();
    }

}

public enum VirtualCameras
{
    STANDARD = 0,
    FIRST_PERSON = 1,
    DRONE = 2
}
