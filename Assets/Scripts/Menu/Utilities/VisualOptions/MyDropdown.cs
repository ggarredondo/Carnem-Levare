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

    private UnityAction<int> changeValue;
    private UnityAction transition;

    public int Value { get => dropdown.value; set => dropdown.value = value; }

    public void SetValue(string value)
    {
        dropdown.value = dropdown.options.FindIndex(option => option.text == value);
    }

    public string GetText(int value) { return dropdown.options[value].text; }

    private void SetActions()
    {
        changeValue = (int value) => trigger.Invoke(value);
        transition = () =>
        {
            EventSystem.current.SetSelectedGameObject(dropdown.gameObject);
            GameManager.Audio.Play("PressButton");
        };
    }

    public override void Initialize()
    {
        AddOptions();
        SetActions();
        base.Initialize();
        AddListener();
    }

    private void AddOptions()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public override void AddListener()
    {
        dropdown.onValueChanged.AddListener(changeValue);
    }

    public override void RemoveListener()
    {
        dropdown.onValueChanged.RemoveListener(changeValue);
    }

    public override void SetButtonAction()
    {
        button.onClick.AddListener(transition);
    }

    public override void SetDependency()
    {
        dependency.onValueChanged.AddListener((bool value) =>
        {
            if (inverseDependency)
                value = !value;

            if (value)
            {
                ChangeColor(ACTIVE_COLOR);
                button.onClick.AddListener(transition);
                dropdown.interactable = true;
            }
            else
            {
                ChangeColor(INACTIVE_COLOR);
                button.onClick.RemoveListener(transition);
                dropdown.interactable = false;
            }
        });
    }

    public override void ChangeColor(Color32 color)
    {
        base.ChangeColor(color);

        ColorBlock cb_dropdown = dropdown.colors;
        cb_dropdown.normalColor = color;
        dropdown.colors = cb_dropdown;
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
