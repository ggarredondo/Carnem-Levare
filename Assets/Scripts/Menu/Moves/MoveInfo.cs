using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MoveInfo : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float yPositionDifference;
    [Range(0, 1)] [SerializeField] private float lerpDuration;

    [Header("Requirements")]
    [SerializeField] private TMP_Text names;
    [SerializeField] private TMP_Text data;

    private bool isMoving;

    private RectTransform rectTransform;

    public void Initialize()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void UpdateInfoBox(ref Move move)
    {
        if(!isMoving)
            Movement();

        UpdateText(in move.StringData);
    }

    private void UpdateText(in List<string> list)
    {
        names.text = "";
        data.text = "";

        for(int i = 0; i < list.Count; i += 2)
        {
            names.text += list[i] + "\n";
            data.text += list[i + 1] + "\n";
        }
    }

    private async void Movement()
    {
        isMoving = true;
        await LerpRectTransform(rectTransform, new Vector3(rectTransform.localPosition.x,
                                                           rectTransform.localPosition.y + yPositionDifference, 
                                                           rectTransform.localPosition.z), lerpDuration);

        await LerpRectTransform(rectTransform, new Vector3(rectTransform.localPosition.x,
                                                   rectTransform.localPosition.y - yPositionDifference,
                                                   rectTransform.localPosition.z), lerpDuration);
        isMoving = false;
    }

    private async Task LerpRectTransform(RectTransform rectTransform, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = rectTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

            await Task.Yield();
        }

        rectTransform.localPosition = targetPosition;
    }
}
