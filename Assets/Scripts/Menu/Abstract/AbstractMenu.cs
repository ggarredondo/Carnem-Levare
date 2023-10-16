using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public abstract class AbstractMenu : MonoBehaviour
{
    [System.Serializable]
    protected struct ToggleData
    {
        public Button button;
        public Toggle toggle;
    }

    private TMP_Dropdown actualDropDown;

    [Header("Requirements")]
    [SerializeField] protected GameObject firstSelected;
    [SerializeField] protected List<Tuple<Button, Selectable>> transitions;

    public TMP_Dropdown ActualDropDown { get => actualDropDown; set => actualDropDown = value; }

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
        SetTransitions();
        Configure();
    }

    private void SetTransitions()
    {
        transitions.ForEach(tuple => tuple.Item1.onClick.AddListener(() => 
        { 
            EventSystem.current.SetSelectedGameObject(tuple.Item2.gameObject);
            GameManager.Audio.Play("PressButton");

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
            GameManager.Audio.Play("SelectButton");
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

    protected void ChangeColor(ref Button selectable, Color32 color)
    {
        ColorBlock cb = selectable.colors;
        cb.normalColor = color;
        selectable.colors = cb;
    }

    protected void ChangeColor(ref Toggle selectable, Color32 color)
    {
        ColorBlock cb = selectable.colors;
        cb.normalColor = color;
        selectable.colors = cb;
    }

    protected void ChangeColorTransitions(int index, Color32 color)
    {
        ColorBlock cb1 = transitions[index].Item1.colors;
        cb1.normalColor = color;
        transitions[index].Item1.colors = cb1;

        ColorBlock cb2 = transitions[index].Item2.colors;
        cb2.normalColor = color;
        transitions[index].Item2.colors = cb2;
    }

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
