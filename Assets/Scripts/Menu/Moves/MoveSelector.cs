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
    [SerializeField] private MoveAssignment moveAssignment;

    private List<RectTransform> moveBlocks = new();
    private List<int> selectedIndex;
    private int actualIndex = 0;

    private Vector3 initialScale;
    private float initialAlpha;

    public void ChangeInputMap()
    {
        moveAssignment.UpdateInput(ref moveBlocks);
    }

    public async void SelectBlock()
    {
        LerpRectTransform(moveBlocks[actualIndex],
                          selectPosition,
                          new Vector3(selectScale, selectScale, 0),
                          selectLerpDuration);

        LerpColor(moveBlocks[actualIndex].GetComponent<CanvasGroup>(),
                  1,
                  selectLerpDuration);

        await moveAssignment.StartChangeInput(actualIndex);
        moveAssignment.UpdateInput(ref moveBlocks);
        DeselectBlock();
    }

    public void DeselectBlock()
    {
        LerpRectTransform(moveBlocks[actualIndex],
                          new Vector3(0,0,0),
                          initialScale,
                          selectLerpDuration);

        LerpColor(moveBlocks[actualIndex].GetComponent<CanvasGroup>(),
                  initialAlpha,
                  selectLerpDuration);

        moveAssignment.EndChangeInput();
    }

    public bool RightMoveBlock()
    {
        if (actualIndex + 1 < moveBlocks.Count)
        {
            actualIndex++;
            moveAssignment.EndChangeInput();
            UpdateSelectedBlocks();
            return true;
        }
        else return false;
    }

    public bool LeftMoveBlock()
    {
        if (actualIndex - 1 >= 0)
        {
            actualIndex--;
            moveAssignment.EndChangeInput();
            UpdateSelectedBlocks();
            return true;
        }
        else return false;
    }

    public int GetActualIndex()
    {
        return actualIndex;
    }

    public void Initialize(ref List<Move> moves, ref InputController inputController)
    {
        InitializeBlockIndex();
        InitializeMoveBlocks(ref moves);
        UpdateSelectedBlocks();
        moveAssignment.Initialize(ref moveBlocks, ref inputController);
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
            tmp.GetComponentsInChildren<Image>()[1].sprite = moves[i].Icon;
            moveBlocks.Add(tmp.GetComponent<RectTransform>());

            if(i == 0)
            {
                initialScale = moveBlocks[i].localScale;
                initialAlpha = moveBlocks[i].GetComponent<CanvasGroup>().alpha;
            }

            moveBlocks[i].localPosition = new Vector3(selectedIndex[^1] * distanceBetweenBlocks, 0, 0);

            float scale = initialScale.x - selectedIndex[^1] * scaleDifference;
            moveBlocks[i].localScale = new Vector3(scale, scale, 0);

            moveBlocks[i].GetComponent<CanvasGroup>().alpha = 0;
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

    private async void LerpColor(CanvasGroup canvasGroup, float targetAlpha, float duration)
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

                LerpColor(moveBlocks[actualIndex + i].GetComponent<CanvasGroup>(),
                          alpha,
                          lerpDuration);
            }
        });
    }
}
