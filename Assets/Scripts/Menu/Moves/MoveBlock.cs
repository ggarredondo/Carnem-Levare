using UnityEngine;
using TMPro;
using LerpUtilities;
using System.Threading.Tasks;

public class MoveBlock : MonoBehaviour
{
    [SerializeField] private GameObject inputGameobject;
    [SerializeField] private GameObject isNewMove;
    [SerializeField] private TMP_Text text;

    [System.NonSerialized] public RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private int ID;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void AsignInput(string input, int ID)
    {
        inputGameobject.SetActive(true);
        text.font = GlobalMenuVariables.Instance.inputFonts[GameManager.InputDetection.controlSchemeIndex];

        if (input != "-" && input != "") text.text += input;
        else
        {
            text.font = GlobalMenuVariables.Instance.inputFonts[0];
            text.text = "M";
        }

        this.ID = ID;
        isNewMove.SetActive(DataSaver.games[DataSaver.currentGameSlot].newMoves[ID]);
    }

    public void Disable()
    {
        inputGameobject.SetActive(false);
        text.text = "";
    }

    public void CheckIfNew()
    {
        if (isNewMove.activeSelf)
        {
            DataSaver.games[DataSaver.currentGameSlot].newMoves[ID] = false;
            isNewMove.SetActive(false);
        }
    }

    public async Task LerpScale(Vector3 targetScale, float duration)
    {
        await Lerp.Scale(rectTransform, targetScale, duration);
    }

    public async void LerpRectTransform(Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        await Lerp.RectTransform(rectTransform, targetPosition, targetScale, duration);
    }

    public async void LerpColor(float targetAlpha, float duration)
    {
        await Lerp.CanvasAlpha(canvasGroup, targetAlpha, duration);
    }
}
