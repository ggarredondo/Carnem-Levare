using UnityEngine;

public class CameraTargets : MonoBehaviour
{
    [SerializeField] private Tuple<Transform>[] targets;

    public Transform GetTarget(int camera, bool alternative)
    {
        return alternative == true ? targets[camera].Item2 : targets[camera].Item1;
    }
}
