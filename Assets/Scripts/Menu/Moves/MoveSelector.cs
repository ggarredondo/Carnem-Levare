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

    private void UpdateSelectedBlocks()
    {
        moveBlocks.ForEach(b => b.gameObject.SetActive(false));

        selectedIndex.ForEach(i =>
        {
            if (actualIndex + i >= 0 && actualIndex + i < moveBlocks.Count)
            {
                moveBlocks[actualIndex + i].gameObject.SetActive(true);

                moveBlocks[actualIndex + i].localPosition = new Vector3(i * distanceBetweenBlocks, 0, 0);

                float scale = initialScale.x - Mathf.Abs(i) * scaleDifference;
                moveBlocks[actualIndex + i].localScale = new Vector3(scale, scale, 0);

                Image current = moveBlocks[actualIndex + i].GetComponent<Image>();
                float alpha = initialAlpha - Mathf.Abs(i) * alphaDifference;
                current.color = new Color(255, 255, 255, alpha); 
            }
        });
    }
}
