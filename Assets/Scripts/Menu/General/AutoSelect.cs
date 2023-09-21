using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AutoSelect : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerUpHandler
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
        if (inputButton)
        {
            GameManager.InputUtilities.ControlsChangedEvent += ChangeInputFont;
            ChangeInputFont();
        }
    }

    private void OnDestroy()
    {
        if (inputButton) GameManager.InputUtilities.ControlsChangedEvent -= ChangeInputFont;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (GameManager.InputUtilities.ControlSchemeIndex == 0 || GameManager.InputUtilities.PreviousCustomControlScheme == InputDevice.KEYBOARD)
        {
            GameManager.InputUtilities.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            GameManager.AudioController.Play("SelectButton");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mouseCanSelect)
            GameManager.AudioController.Play("SelectButton");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (mouseCanSelect)
            EventSystem.current.SetSelectedGameObject(null);
    }

    private void ChangeInputFont()
    {
        //Change Font
        transform.GetChild(0).GetComponent<TMP_Text>().font = GlobalMenuVariables.Instance.inputFonts[GameManager.InputUtilities.ControlSchemeIndex];

        //Asign the correct word
        string mappingKey = GameManager.InputUtilities.ObtainAllowedMapping(gameObject.name);

        if (mappingKey != "-" && mappingKey != "") transform.GetChild(0).GetComponent<TMP_Text>().text = mappingKey;
        else
        {
            transform.GetChild(0).GetComponent<TMP_Text>().font = GlobalMenuVariables.Instance.inputFonts[0];
            transform.GetChild(0).GetComponent<TMP_Text>().text = "M";
        }
    }
}
