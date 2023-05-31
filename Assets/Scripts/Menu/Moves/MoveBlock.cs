using UnityEngine;
using TMPro;

public class MoveBlock : MonoBehaviour
{
    [SerializeField] private GameObject inputGameobject;
    [SerializeField] private TMP_Text text;

    public void AsignInput(string input)
    {
        inputGameobject.SetActive(true);
        text.font = GlobalMenuVariables.Instance.inputFonts[GameManager.InputDetection.controlSchemeIndex];
        text.text = input;
    }

    public void Disable()
    {
        inputGameobject.SetActive(false);
    }
}
