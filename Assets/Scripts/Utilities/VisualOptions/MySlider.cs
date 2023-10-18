using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MySlider : MySelectable
{
    [SerializeField] private Slider slider;
    [SerializeField] private UnityEvent<float> trigger;

    public float Value { get => slider.value; set => slider.value = value; }

    public override void Initialize()
    {
        AddListener();
    }

    public override void AddListener()
    {
        slider.onValueChanged.AddListener((float value) => trigger.Invoke(value));
    }

    public override void RemoveListener()
    {
        slider.onValueChanged.RemoveAllListeners();
    }

    public override void ChangeColor(Color32 color)
    {
        base.ChangeColor(color);

        ColorBlock cb_slider = slider.colors;
        cb_slider.normalColor = color;
        slider.colors = cb_slider;
    }
}
