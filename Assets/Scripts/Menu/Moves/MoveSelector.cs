using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSelector : MonoBehaviour
{
    private const int NUM_SELECTED_BLOCKS = 7;

    [Header("Parameters")]
    [SerializeField] private float distanceBetweenBlocks;
    [Range(0, 1)] [SerializeField] private float scaleDifference;
    [Range(0, 1)] [SerializeField] private float alphaDifference;
    [Range(0, 1)] [SerializeField] private float lerpDuration;

    [Header("Requirements")]
    [SerializeField] private List<Move> moves;
    [SerializeField] private GameObject moveBlockPrefab;
    [SerializeField] private InputReader inputReader;

    private List<RectTransform> moveBlocks = new();
    private List<int> selectedIndex = new(NUM_SELECTED_BLOCKS);
    private int actualIndex = 0;

    private Vector3 initialScale;
    private float initialAlpha;

    private void OnEnable()
    {
        inputReader.DPAdEvent += DPad;
    }

    private void OnDisable()
    {
        inputReader.DPAdEvent -= DPad;
    }

    private void Awake()
    {
        InitializeBlockIndex();
        InitializeMoveBlocks();
        UpdateSelectedBlocks();
    }

    private void InitializeBlockIndex()
    {
        int start = NUM_SELECTED_BLOCKS / 2;
        start *= -1;

        for (int i = 0; i < NUM_SELECTED_BLOCKS; i++)
        {
            selectedIndex.Add(start);
            start++;
        }
    }

    private void InitializeMoveBlocks()
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

            moveBlocks[i].localPosition = new Vector3(900, 0, 0);
            moveBlocks[i].localScale = new Vector3(0.4f, 0.4f, 0);
            moveBlocks[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
    }

    private void DPad(Vector2 value)
    {
        if (value == new Vector2(-1, 0))
            LeftMoveBlock();
        if (value == new Vector2(1, 0))
            RightMoveBlock();
    }

    private void RightMoveBlock()
    {
        if (actualIndex + 1 < moveBlocks.Count)
        {
            actualIndex++;
            UpdateSelectedBlocks();
        }
    }

    private void LeftMoveBlock()
    {
        if (actualIndex - 1 >= 0)
        {
            actualIndex--;
            UpdateSelectedBlocks();
        }
    }

    public IEnumerator LerpRectTransform(RectTransform rectTransform, Vector3 targetPosition, Vector3 targetScale, float duration)
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

            yield return null;
        }

        rectTransform.localPosition = targetPosition;
        rectTransform.localScale = targetScale;
    }

    public IEnumerator LerpColor(Image image, Color color, float duration)
    {
        Color startColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            image.color = Color.Lerp(startColor, color, t);

            yield return null;
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

                StartCoroutine(LerpRectTransform(moveBlocks[actualIndex + i],
                                                      new Vector3(i * distanceBetweenBlocks, 0, 0),
                                                      new Vector3(scale, scale, 0),
                                                      lerpDuration));

                StartCoroutine(LerpColor(moveBlocks[actualIndex + i].GetComponent<Image>(),
                                         new Color(255, 255, 255, alpha),
                                         lerpDuration));
            }
        });
    }
}
