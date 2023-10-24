using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Axis { x = 0, y = 1, z = 2 }

[System.Serializable]
public class SelfUpdatingBar
{
    [SerializeField] private Image animated, instant;
    [Tooltip("Optional")] [SerializeField] private TMP_Text number;
    [SerializeField] private float updateSpeed = 5f;
    [SerializeField] private Axis axis;
    private float animatedValue;

    /// <summary>
    /// Update image scale in one axis corresponding to a percentage given by
    /// a current and max value.
    /// </summary>
    private void UpdateImage(float value, float maxValue, Image image, Axis axis)
    {
        Vector3 newScale = new Vector3(1f, 1f, 1f);
        newScale[(int)axis] = value / (maxValue == 0f ? 1f : maxValue);
        image.transform.localScale = newScale;
    }

    public void UpdateBar(float value, float maxValue)
    {
        animatedValue = Mathf.Lerp(animatedValue, value, updateSpeed * Time.deltaTime);
        UpdateImage(animatedValue, maxValue, animated, axis);
        UpdateImage(value, maxValue, instant, axis);
        if (number) number.text = value + "/" + maxValue;
    }
}
