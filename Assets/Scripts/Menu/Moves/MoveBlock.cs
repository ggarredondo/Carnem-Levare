using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class MoveBlock : MonoBehaviour
{
    [SerializeField] private GameObject inputGameobject;
    [SerializeField] private TMP_Text text;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void AsignInput(string input)
    {
        inputGameobject.SetActive(true);
        text.font = GlobalMenuVariables.Instance.inputFonts[GameManager.InputDetection.controlSchemeIndex];
        text.text += input;
    }

    public void Disable()
    {
        inputGameobject.SetActive(false);
        text.text = "";
    }

    public async void LerpRectTransform(Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        Vector3 startPosition = rectTransform.localPosition;
        Vector3 startScale = rectTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);

            await Task.Yield();
        }

        rectTransform.localPosition = targetPosition;
        rectTransform.localScale = targetScale;
    }

    public async void LerpColor(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            await Task.Yield();
        }

        canvasGroup.alpha = targetAlpha;
    }
}
