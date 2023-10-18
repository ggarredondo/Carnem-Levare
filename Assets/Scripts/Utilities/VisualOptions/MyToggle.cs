using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MyToggle : MySelectable
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private UnityEvent<bool> trigger;

    public bool Value { get => toggle.isOn; set => toggle.isOn = value; }

    public override void Initialize()
    {
        AddListener();
    }

    public override void AddListener()
    {
        toggle.onValueChanged.AddListener((bool value) => trigger.Invoke(value));
        button.onClick.AddListener(() => toggle.isOn = !toggle.isOn);
    }

    public override void RemoveListener()
    {
        toggle.onValueChanged.RemoveAllListeners();
        button.onClick.RemoveAllListeners();
    }

    public override void ChangeColor(Color32 color)
    {
        base.ChangeColor(color);

        ColorBlock cb_toggle = toggle.colors;
        cb_toggle.normalColor = color;
        toggle.colors = cb_toggle;
    }
}
