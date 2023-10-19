using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;

public abstract class AbstractMenu : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] protected GameObject firstSelected;

    protected List<ITransition> newTransitions = new();
    protected readonly List<MySelectable> elements = new();

    private void ObtainByReflection()
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (FieldInfo field in fields)
        {
            if (typeof(MySelectable).IsAssignableFrom(field.FieldType))
            {
                elements.Add((MySelectable)field.GetValue(this));
            }

            if (typeof(ITransition).IsAssignableFrom(field.FieldType))
            {
                newTransitions.Add((ITransition)field.GetValue(this));
            }
        }
    }

    protected virtual void OnDisable()
    {
        if(EventSystem.current != null && GameManager.Input.ControlSchemeIndex == 0)
            firstSelected = EventSystem.current.currentSelectedGameObject;
    }

    public void Initialized()
    {
        if (GameManager.Input.ControlSchemeIndex == 0 || GameManager.Input.PreviousCustomControlScheme == InputDevice.KEYBOARD)
            EventSystem.current.SetSelectedGameObject(firstSelected);

        GameManager.Input.SetSelectedGameObject(firstSelected);
    }

    private void Start()
    {
        ObtainByReflection();
        SetTransitions();
        Configure();
    }

    private void SetTransitions()
    {
        newTransitions.ForEach(element => element.SetTransition());
    }

    public bool HasTransition()
    {
        foreach (ITransition transition in newTransitions)
            if (transition.HasTransition()) return true;

        return false;
    }

    protected abstract void Configure();

    protected void Slider(ref float save, float value, bool hasSound)
    {
        save = value;
        if(hasSound) GameManager.Audio.Slider();
        GameManager.Save.ApplyChanges();
    }

    protected void Toggle(ref bool save, bool value)
    {
        save = value;
        GameManager.Audio.Play("PressButton");
        GameManager.Save.ApplyChanges();
    }

    protected void Dropdown(ref string save, string value)
    {
        save = value;
        GameManager.Audio.Play("PressButton");
        GameManager.Save.ApplyChanges();
    }

    protected void Dropdown(ref int save, int value)
    {
        save = value;
        GameManager.Audio.Play("PressButton");
        GameManager.Save.ApplyChanges();
    }
}
