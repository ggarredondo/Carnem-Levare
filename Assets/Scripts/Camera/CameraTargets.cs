using UnityEngine;

public class CameraTargets : MonoBehaviour
{
    [SerializeField] private Tuple<Transform>[] targets;

    public Transform GetDefaultTarget(CameraType camera)
    {
        return targets[(int) camera].Item1;
    }

    public Transform GetAlternativeTarget(CameraType camera)
    {
        return targets[(int)camera].Item2;
    }
}
