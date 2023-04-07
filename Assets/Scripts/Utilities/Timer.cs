using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private bool enable = false;
    [SerializeField] [ReadOnlyField] private float currentTime = 0f;

    public void StartTimer() { currentTime = 0f; enable = true; }
    public void StopTimer() { currentTime = 0f; enable = false; }

    private void Update() {
        if (enable) currentTime += Time.deltaTime * 1000f;
        else currentTime = 0f;
    }
}