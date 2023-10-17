using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MyToggle : ISelectable
{
    [SerializeField] private Button button;
    [SerializeField] private Toggle toggle;
    [SerializeField] private UnityEvent<bool> trigger;

    public bool Value { get => toggle.isOn; set => toggle.isOn = value; }

    public void Initialize()
    {
        AddListener();
    }

    public void AddListener()
    {
        toggle.onValueChanged.AddListener((bool value) => trigger.Invoke(value));
        button.onClick.AddListener(() => toggle.isOn = !toggle.isOn);
    }

    public void RemoveListener()
    {
        toggle.onValueChanged.RemoveAllListeners();
        button.onClick.RemoveAllListeners();
    }
}
