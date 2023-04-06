using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool enable = false;
    [SerializeField] private float currentTime = 0f;

    private void Update() {
        if (enable) currentTime += Time.deltaTime * 1000f;
        else currentTime = 0f;
    }
}