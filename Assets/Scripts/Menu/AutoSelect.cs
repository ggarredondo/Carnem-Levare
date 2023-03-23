using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AutoSelect : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] private int typeButton;
    [SerializeField] private bool mouseCanSelect;
    [SerializeField] private bool inputButton;

    private void Start()
    {
        //Initialize font
        GetComponent<TMP_Text>().font = GlobalMenuVariables.Instance.font;

        //Initialize buttons colors
        ColorBlock colors = GetComponent<Button>().colors;

        switch (typeButton)
        {
            case 1:
                colors.selectedColor = GlobalMenuVariables.Instance.selectedButtonColor;
                colors.highlightedColor = GlobalMenuVariables.Instance.highlightedButtonColor;
                break;
            case 2:
                colors.selectedColor = GlobalMenuVariables.Instance.selectedButtonColor2;
                colors.highlightedColor = GlobalMenuVariables.Instance.highlightedButtonColor2;
                break;
        }

        GetComponent<Button>().colors = colors;
    }

    private void OnEnable()
    {
        if(inputButton) ControlSaver.StaticEvent += ChangeInputFont;
    }

    private void OnDisable()
    {
        if(inputButton) ControlSaver.StaticEvent -= ChangeInputFont;
    }

    private void ChangeInputFont()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().font = GlobalMenuVariables.Instance.inputFonts[ControlSaver.controlSchemeIndex];
        transform.GetChild(0).GetComponent<TMP_Text>().text = ControlSaver.ObtainMapping(gameObject.name);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.uiSfxSounds.Play("SelectButton");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mouseCanSelect)
            GetComponent<Selectable>().Select();
    }
}
