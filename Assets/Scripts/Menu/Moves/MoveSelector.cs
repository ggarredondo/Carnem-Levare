using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MoveSelector : MonoBehaviour
{
    [Header("Right/Left Parameters")]
    [SerializeField] private int numSelectedBlocks = 7;
    [SerializeField] private float distanceBetweenBlocks;
    [Range(0, 1)] [SerializeField] private float scaleDifference;
    [Range(0, 1)] [SerializeField] private float alphaDifference;
    [Range(0, 1)] [SerializeField] private float lerpDuration;

    [Header("Select Parameters")]
    [SerializeField] private Vector3 selectPosition;
    [SerializeField] private float selectScale;
    [Range(0, 1)] [SerializeField] private float selectLerpDuration;

    [Header("Requirements")]
    [SerializeField] private GameObject moveBlockPrefab;

    private readonly List<RectTransform> moveBlocks = new();
    private List<int> selectedIndex;
    private int actualIndex = 0;

    private Vector3 initialScale;
    private float initialAlpha;

    public void SelectBlock()
    {
        LerpRectTransform(moveBlocks[actualIndex],
                          selectPosition,
                          new Vector3(selectScale, selectScale, 0),
                          selectLerpDuration);

        LerpColor(moveBlocks[actualIndex].GetComponent<Image>(),
                  new Color(1, 1, 1, 1),
                  selectLerpDuration);
    }

    public void DeselectBlock()
    {
        LerpRectTransform(moveBlocks[actualIndex],
                          new Vector3(0,0,0),
                          initialScale,
                          selectLerpDuration);

        LerpColor(moveBlocks[actualIndex].GetComponent<Image>(),
                  new Color(1, 1, 1, initialAlpha),
                  selectLerpDuration);
    }

    public void RightMoveBlock()
    {
        if (actualIndex + 1 < moveBlocks.Count)
        {
            actualIndex++;
            UpdateSelectedBlocks();
        }
    }

    public void LeftMoveBlock()
    {
        if (actualIndex - 1 >= 0)
        {
            actualIndex--;
            UpdateSelectedBlocks();
        }
    }

    public int GetActualIndex()
    {
        return actualIndex;
    }

    public void Initialize(ref List<Move> moves)
    {
        InitializeBlockIndex();
        InitializeMoveBlocks(ref moves);
        UpdateSelectedBlocks();
    }

    private void InitializeBlockIndex()
    {
        selectedIndex = new(numSelectedBlocks);
        int start = numSelectedBlocks / 2;
        start *= -1;

        for (int i = 0; i < numSelectedBlocks; i++)
        {
            selectedIndex.Add(start);
            start++;
        }
    }

    private void InitializeMoveBlocks(ref List<Move> moves)
    {
        for (int i = 0; i < moves.Count; i++)
        {
            GameObject tmp = Instantiate(moveBlockPrefab);
            tmp.transform.SetParent(gameObject.transform, false);
            moveBlocks.Add(tmp.GetComponent<RectTransform>());

            if(i == 0)
            {
                initialScale = moveBlocks[i].localScale;
                initialAlpha = moveBlocks[i].GetComponent<Image>().color.a;
            }

            moveBlocks[i].localPosition = new Vector3(selectedIndex[^1] * distanceBetweenBlocks, 0, 0);

            float scale = initialScale.x - selectedIndex[^1] * scaleDifference;
            moveBlocks[i].localScale = new Vector3(scale, scale, 0);

            moveBlocks[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
    }

    private async void LerpRectTransform(RectTransform rectTransform, Vector3 targetPosition, Vector3 targetScale, float duration)
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

    private async void LerpColor(Image image, Color color, float duration)
    {
        Color startColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            image.color = Color.Lerp(startColor, color, t);

            await Task.Yield();
        }

        image.color = color;
    }

    private void UpdateSelectedBlocks()
    {
        moveBlocks.ForEach(b => b.gameObject.SetActive(false));

        selectedIndex.ForEach(i =>
        {
            if (actualIndex + i >= 0 && actualIndex + i < moveBlocks.Count)
            {
                moveBlocks[actualIndex + i].gameObject.SetActive(true);

                float scale = initialScale.x - Mathf.Abs(i) * scaleDifference;
                float alpha = initialAlpha - Mathf.Abs(i) * alphaDifference;

                LerpRectTransform(moveBlocks[actualIndex + i],
                                  new Vector3(i * distanceBetweenBlocks, 0, 0),
                                  new Vector3(scale, scale, 0),
                                  lerpDuration);

                LerpColor(moveBlocks[actualIndex + i].GetComponent<Image>(),
                          new Color(255, 255, 255, alpha),
                          lerpDuration);
            }
        });
    }
}
