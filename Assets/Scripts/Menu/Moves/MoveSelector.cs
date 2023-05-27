using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSelector : MonoBehaviour
{
    private const int NUM_SELECTED_BLOCKS = 5;

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
        }

        initialScale = moveBlocks[0].localScale;
        initialAlpha = moveBlocks[0].GetComponent<Image>().color.a;
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

    public IEnumerator LerpRectTransformAsync(RectTransform rectTransform, Vector3 targetPosition, Vector3 targetScale, float duration)
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

    private void UpdateSelectedBlocks()
    {
        moveBlocks.ForEach(b => b.gameObject.SetActive(false));

        selectedIndex.ForEach(i =>
        {
            if (actualIndex + i >= 0 && actualIndex + i < moveBlocks.Count)
            {
                moveBlocks[actualIndex + i].gameObject.SetActive(true);

                float scale = initialScale.x - Mathf.Abs(i) * scaleDifference;
                StartCoroutine(LerpRectTransformAsync(moveBlocks[actualIndex + i],
                                                      new Vector3(i * distanceBetweenBlocks, 0, 0),
                                                      new Vector3(scale, scale, 0),
                                                      lerpDuration));

                Image current = moveBlocks[actualIndex + i].GetComponent<Image>();
                float alpha = initialAlpha - Mathf.Abs(i) * alphaDifference;
                current.color = new Color(255, 255, 255, alpha); 
            }
        });
    }
}
