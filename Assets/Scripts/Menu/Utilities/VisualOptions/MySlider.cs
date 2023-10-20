using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class MySlider : MySelectable, ITransition
{
    [SerializeField] private Slider slider;
    [SerializeField] private UnityEvent<float> trigger;

    private UnityAction<float> changeValue;
    private UnityAction transition;

    private CanvasGroup sliderCanvasGroup;

    public float Value { get => slider.value; set => slider.value = value; }

    private void SetActions()
    {
        changeValue = (float value) => trigger.Invoke(value);
        transition = () =>
        {
            EventSystem.current.SetSelectedGameObject(slider.gameObject);
            GameManager.Audio.Play("PressButton");
        };
    }

    public override void Initialize()
    {
        sliderCanvasGroup = slider.gameObject.GetComponent<CanvasGroup>();
        SetActions();
        base.Initialize();
        AddListener();
    }

    public override void AddListener()
    {
        slider.onValueChanged.AddListener(changeValue);
    }

    public override void RemoveListener()
    {
        slider.onValueChanged.RemoveListener(changeValue);
    }

    public override void SetButtonAction()
    {
        button.onClick.AddListener(transition);
    }

    public override void SetDependency()
    {
        dependency.onValueChanged.AddListener((bool value) =>
        {
            if(inverseDependency)
                value = !value;

            if (value)
            {
                ChangeColor(ACTIVE_COLOR);
                button.onClick.AddListener(transition);
                slider.interactable = true;
            }
            else
            {
                ChangeColor(INACTIVE_COLOR);
                button.onClick.RemoveListener(transition);
                slider.interactable = false;
            }
        });
    }

    public override void ChangeColor(Color32 color)
    {
        base.ChangeColor(color);

        sliderCanvasGroup.alpha = color.a / 255.0f;
    }

    public void Return()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    public bool HasTransition()
    {
        bool transition = EventSystem.current.currentSelectedGameObject == slider.gameObject;

        if (transition) Return();

        return transition;
    }
}
