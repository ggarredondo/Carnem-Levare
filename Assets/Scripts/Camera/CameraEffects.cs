using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private List<CameraMovement> cameraEffects;

    public void Initialize(ref CinemachineVirtualCamera vcam, ref Player player)
    {
        foreach(CameraMovement movement in cameraEffects)
        {
            movement.Initialize(ref vcam);
            movement.UpdateCondition(ref player);
        }
    }
}
