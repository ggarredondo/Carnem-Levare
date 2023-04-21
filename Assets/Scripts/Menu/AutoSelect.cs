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

    private void Awake()
    {
        if (inputButton) ControlSaver.Instance.StaticEvent += ChangeInputFont;
    }

    private void OnDestroy()
    {
        if (inputButton) ControlSaver.Instance.StaticEvent -= ChangeInputFont;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (ControlSaver.Instance.controlSchemeIndex == 0)
        {
            AudioManager.Instance.uiSfxSounds.Play("SelectButton");
            ControlSaver.Instance.selected = gameObject;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mouseCanSelect)
            AudioManager.Instance.uiSfxSounds.Play("SelectButton");
    }

    private void ChangeInputFont()
    {
        //Change Font
        transform.GetChild(0).GetComponent<TMP_Text>().font = GlobalMenuVariables.Instance.inputFonts[ControlSaver.Instance.controlSchemeIndex];

        //Asign the correct word
        string mappingKey = ControlSaver.Instance.ObtainMapping(gameObject.name);

        if (mappingKey != "-" && mappingKey != "") transform.GetChild(0).GetComponent<TMP_Text>().text = ControlSaver.Instance.ObtainMapping(gameObject.name);
        else
        {
            transform.GetChild(0).GetComponent<TMP_Text>().font = GlobalMenuVariables.Instance.inputFonts[0];
            transform.GetChild(0).GetComponent<TMP_Text>().text = "M";
        }
    }
}
