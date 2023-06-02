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
    }

    private void Awake()
    {
        if (inputButton) GameManager.InputDetection.controlsChangedEvent += ChangeInputFont;
    }

    private void OnDestroy()
    {
        if (inputButton) GameManager.InputDetection.controlsChangedEvent -= ChangeInputFont;
    }

    private void ChangeInputFont()
    {
        tmpText.font = GlobalMenuVariables.Instance.inputFonts[GameManager.InputDetection.controlSchemeIndex];

        string mappingKey = GameManager.InputMapping.ObtainMapping(gameObject.name);

        if (mappingKey != "-" && mappingKey != "") tmpText.text = mappingKey;
        else
        {
            tmpText.font = GlobalMenuVariables.Instance.inputFonts[0];
            tmpText.text = "M";
        }
    }
}
