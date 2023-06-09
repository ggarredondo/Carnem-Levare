using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LerpUtilities
{
    public class Lerp
    {
        public static async Task CanvasAlpha(CanvasGroup canvasGroup, float targetAlpha, float duration)
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

        public static async Task CanvasAlpha_Unscaled(CanvasGroup canvasGroup, float targetAlpha, float duration)
        {
            float startAlpha = canvasGroup.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

                await Task.Yield();
            }

            canvasGroup.alpha = targetAlpha;
        }

        public static async Task RectTransform(RectTransform rectTransform, Vector3 targetPosition, Vector3 targetScale, float duration)
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

        public static async Task RectTransform_Color(RectTransform rectTransform, Image image, Vector3 targetPosition, Color targetColor, float duration)
        {
            Vector3 startPosition = rectTransform.localPosition;
            Color startColor = image.color;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
                image.color = Color.Lerp(startColor, targetColor, t);

                await Task.Yield();
            }

            rectTransform.localPosition = targetPosition;
            image.color = targetColor;
        }

        public static async Task Text_Color(TMP_Text text, Color targetColor, float duration)
        {
            Color startColor = text.color;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                text.color = Color.Lerp(startColor, targetColor, t);

                await Task.Yield();
            }

            text.color = targetColor;
        }
    }
}
