using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[System.Serializable]
public class MyDropdown : MySelectable, ITransition
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private List<string> options;
    [SerializeField] private UnityEvent<int> trigger;

    public int Value { get => dropdown.value; set => dropdown.value = value; }

    public void SetValue(string value)
    {
        dropdown.value = dropdown.options.FindIndex(option => option.text == value);
    }

    public string GetText(int value) { return dropdown.options[value].text; }

    public override void Initialize()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        AddListener();
    }

    public override void AddListener()
    {
        dropdown.onValueChanged.AddListener((int value) => trigger.Invoke(value));
    }

    public override void RemoveListener()
    {
        dropdown.onValueChanged.RemoveAllListeners();
    }

    public override void ChangeColor(Color32 color)
    {
        base.ChangeColor(color);

        ColorBlock cb_dropdown = dropdown.colors;
        cb_dropdown.normalColor = color;
        dropdown.colors = cb_dropdown;
    }

    public void SetTransition()
    {
        button.onClick.AddListener(() =>
        {
            EventSystem.current.SetSelectedGameObject(dropdown.gameObject);
            GameManager.Audio.Play("PressButton");
        });
    }

    public void Return()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    public bool HasTransition()
    {
        bool transition = EventSystem.current.currentSelectedGameObject == dropdown.gameObject;
        bool isExpanded = dropdown.IsExpanded;

        if (transition) Return();
        if (isExpanded) { dropdown.Hide(); GameManager.Audio.Play("SelectButton"); }

        return transition || isExpanded;
    }
}
