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
        if(EventSystem.current != null && GameManager.InputDetection.controlSchemeIndex == 0)
            firstSelected = EventSystem.current.currentSelectedGameObject;
    }

    public void Initialized()
    {
        if (GameManager.InputDetection.controlSchemeIndex == 0)
            EventSystem.current.SetSelectedGameObject(firstSelected);

        GameManager.InputDetection.selected = firstSelected;
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
            AudioController.Instance.uiSfxSounds.Play("PressButton");

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
            AudioController.Instance.uiSfxSounds.Play("SelectButton");
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
        if(hasSound) AudioController.Instance.Slider();
        OptionsApplier.apply.Invoke();
    }

    protected void Toggle(ref bool save, bool value)
    {
        save = value;
        AudioController.Instance.uiSfxSounds.Play("PressButton");
        OptionsApplier.apply.Invoke();
    }

    protected void Dropdown(ref string save, string value)
    {
        save = value;
        AudioController.Instance.uiSfxSounds.Play("PressButton");
        OptionsApplier.apply.Invoke();
    }

    protected void Dropdown(ref int save, int value)
    {
        save = value;
        AudioController.Instance.uiSfxSounds.Play("PressButton");
        OptionsApplier.apply.Invoke();
    }
}
