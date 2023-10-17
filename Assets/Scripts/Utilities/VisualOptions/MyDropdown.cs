using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MyDropdown : ISelectable
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private List<string> options;
    [SerializeField] private UnityEvent<int> trigger;

    public int Value { get => dropdown.value; set => dropdown.value = value; }

    public void SetValue(string value)
    {
        dropdown.value = dropdown.options.FindIndex(option => option.text == value);
    }

    public string GetText(int value) { return dropdown.options[value].text; }

    public void Initialize()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        AddListener();
    }

    public void AddListener()
    {
        dropdown.onValueChanged.AddListener((int value) => trigger.Invoke(value));
    }

    public void RemoveListener()
    {
        dropdown.onValueChanged.RemoveAllListeners();
    }
}
