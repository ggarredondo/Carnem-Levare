using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public abstract class AbstractMenu : MonoBehaviour
{
    private TMP_Dropdown actualDropDown;

    [Header("Requirements")]
    [SerializeField] protected GameObject firstSelected;
    [SerializeField] private List<Tuple<Button, Selectable>> transitions;

    public TMP_Dropdown ActualDropDown { get => actualDropDown; set => actualDropDown = value; }

    protected virtual void OnDisable()
    {
        if(EventSystem.current != null && GameManager.InputUtilities.ControlSchemeIndex == 0)
            firstSelected = EventSystem.current.currentSelectedGameObject;
    }

    public void Initialized()
    {
        if (GameManager.InputUtilities.ControlSchemeIndex == 0 || GameManager.InputUtilities.PreviousCustomControlScheme == InputDevice.KEYBOARD)
            EventSystem.current.SetSelectedGameObject(firstSelected);

        GameManager.InputUtilities.SetSelectedGameObject(firstSelected);
    }

    private void Start()
    {
        SetTransitions();
        Configure();
    }

    private void SetTransitions()
    {
        transitions.ForEach(tuple => tuple.Item1.onClick.AddListener(() => 
        { 
            EventSystem.current.SetSelectedGameObject(tuple.Item2.gameObject);
            GameManager.AudioController.Play("PressButton");

            if (tuple.Item2 is TMP_Dropdown item) actualDropDown = item;
        }));
    }

    public bool Return()
    {
        bool canReturn = false;

        if (HasTransition())
        {
            actualDropDown = null;
            EventSystem.current.SetSelectedGameObject(transitions.FirstOrDefault(tuple => tuple.Item2.gameObject == EventSystem.current.currentSelectedGameObject).Item1.gameObject);
            canReturn = true;
        }
        
        if (ActualDropDown != null)
        {
            ActualDropDown.Hide();
            GameManager.AudioController.Play("SelectButton");
            canReturn = true;
        }

        return canReturn;
    }

    protected bool HasTransition()
    {
        if (EventSystem.current.currentSelectedGameObject != null && transitions.Count > 0)
            return transitions.Any(tuple => tuple.Item2.gameObject == EventSystem.current.currentSelectedGameObject);
        else
            return false;
    }

    protected abstract void Configure();

    protected void Slider(ref float save, float value, bool hasSound)
    {
        save = value;
        if(hasSound) GameManager.AudioController.Slider();
        GameManager.Save.ApplyChanges();
    }

    protected void Toggle(ref bool save, bool value)
    {
        save = value;
        GameManager.AudioController.Play("PressButton");
        GameManager.Save.ApplyChanges();
    }

    protected void Dropdown(ref string save, string value)
    {
        save = value;
        GameManager.AudioController.Play("PressButton");
        GameManager.Save.ApplyChanges();
    }

    protected void Dropdown(ref int save, int value)
    {
        save = value;
        GameManager.AudioController.Play("PressButton");
        GameManager.Save.ApplyChanges();
    }
}
