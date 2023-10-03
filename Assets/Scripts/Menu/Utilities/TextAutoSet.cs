using UnityEngine;
using TMPro;

public class TextAutoSet : MonoBehaviour
{
    [SerializeField] private int typeButton;
    [SerializeField] private bool inputButton;
    [SerializeField] private TMP_Text tmpText;

    private void Start()
    {
        switch (typeButton)
        {
            case 1: tmpText.color = GlobalMenuVariables.Instance.selectedButtonColor; 
                break;
            case 2: tmpText.color = GlobalMenuVariables.Instance.selectedButtonColor2;
                break;
        }

        if (inputButton)
        {
            GameManager.Input.ControlsChangedEvent += ChangeInputFont;
            ChangeInputFont();
        }
    }

    private void OnDestroy()
    {
        if (inputButton) GameManager.Input.ControlsChangedEvent -= ChangeInputFont;
    }

    private void ChangeInputFont()
    {
        tmpText.font = GlobalMenuVariables.Instance.ActualInputFont();

        string mappingKey = GameManager.Input.ObtainMapping(gameObject.name);

        if (mappingKey != "-" && mappingKey != "") tmpText.text = mappingKey;
        else
        {
            tmpText.font = GlobalMenuVariables.Instance.inputFonts[0];
            tmpText.text = "M";
        }
    }
}
