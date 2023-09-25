using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private List<CameraEffect> cameraEffects;

    public void Initialize(ref CinemachineVirtualCamera vcam, ref Player player, ref Enemy enemy)
    {
        foreach(CameraEffect movement in cameraEffects)
        {
            movement.Initialize(ref vcam);
            movement.UpdateCondition(ref player, ref enemy);
        }
    }
}
