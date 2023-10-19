using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MyToggle : MySelectable
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private UnityEvent<bool> trigger;

    private UnityAction<bool> changeValue;
    private UnityAction click;

    public bool Value { get => toggle.isOn; set => toggle.isOn = value; }
    public Toggle.ToggleEvent Event { get => toggle.onValueChanged; }

    private void SetActions()
    {
        changeValue = (bool value) => trigger.Invoke(value);
        click = () => toggle.isOn = !toggle.isOn;
    }

    public override void Initialize()
    {
        SetActions();
        base.Initialize();
        AddListener();
    }

    public override void AddListener()
    {
        toggle.onValueChanged.AddListener(changeValue);
    }

    public override void RemoveListener()
    {
        toggle.onValueChanged.RemoveListener(changeValue);
    }

    public override void SetButtonAction()
    {
        button.onClick.AddListener(click);
    }

    public override void SetDependency()
    {
        dependency.onValueChanged.AddListener((bool value) =>
        {
            if (value)
            {
                ChangeColor(ACTIVE_COLOR);
                button.onClick.AddListener(click);
                toggle.interactable = true;
            }
            else
            {
                ChangeColor(INACTIVE_COLOR);
                button.onClick.RemoveListener(click);
                toggle.interactable = false;
            }
        });
    }

    public override void ChangeColor(Color32 color)
    {
        base.ChangeColor(color);

        ColorBlock cb_toggle = toggle.colors;
        cb_toggle.normalColor = color;
        toggle.colors = cb_toggle;
    }
}
