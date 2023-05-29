using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private List<GameObject> HUDMenus;

    [Header("Parameters")]
    [Range(0,1)] [SerializeField] private float lerpDuration;

    private int actualHUDMenu = 0;

    private void OnEnable()
    {
        inputReader.SelectMenuEvent += ChangeHUDMenu;
    }

    private void OnDisable()
    {
        inputReader.SelectMenuEvent -= ChangeHUDMenu;
    }

    public async void ChangeHUDMenu()
    {
        CanvasGroup actualCanvas = HUDMenus[actualHUDMenu].GetComponent<CanvasGroup>();

        if (actualCanvas != null)
            await LerpCanvasAlpha(actualCanvas, 0, 0.1f);

        HUDMenus.ForEach(m => m.SetActive(false));
        actualHUDMenu = (actualHUDMenu + 1) % HUDMenus.Count;
        HUDMenus[actualHUDMenu].SetActive(true);

        actualCanvas = HUDMenus[actualHUDMenu].GetComponent<CanvasGroup>();

        if (actualCanvas != null)
            await LerpCanvasAlpha(actualCanvas, 1, lerpDuration);
    }

    private async Task LerpCanvasAlpha(CanvasGroup canvasGroup,float targetAlpha, float duration)
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
