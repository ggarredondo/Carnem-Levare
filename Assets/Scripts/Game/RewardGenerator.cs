using LerpUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RewardGenerator : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private GameObject moveBlockPrefab;
    [SerializeField] private AddLayoutElements addLayoutElements;

    [Header("Parameters")]
    [SerializeField] private float zoomLerpDuration;
    [SerializeField] private float moveLerpDuration;
    [SerializeField] private float waitBetweenMoves;

    private List<MoveBlock> moveBlocks;
    private List<MoveBlock> moveBlocksToFollow;

    private void Awake()
    {
        moveBlocks = new();
        moveBlocksToFollow = new();
    }

    public async Task GenerateMove(List<Move> moves)
    {
        for (int i = 0; i < moves.Count; i++)
        {
            GameObject followAt = addLayoutElements.AddElement();
            await Task.Delay(System.TimeSpan.FromSeconds(waitBetweenMoves));

            GameObject tmp = Instantiate(moveBlockPrefab);
            tmp.transform.SetParent(gameObject.transform, false);
            tmp.GetComponentsInChildren<Image>()[1].sprite = moves[i].Icon;

            moveBlocks.Add(tmp.GetComponent<MoveBlock>());
            moveBlocksToFollow.Add(followAt.GetComponent<MoveBlock>());

            Move();

            AudioController.Instance.uiSfxSounds.Play("NewMove1");
            await tmp.GetComponent<MoveBlock>().LerpScale(new Vector3(1, 1, 0), zoomLerpDuration);
        }
    }

    private void Move()
    {
        for(int i = 0; i < moveBlocks.Count; i++)
        {
            Vector3 actualPosition = moveBlocksToFollow[i].rectTransform.localPosition;
            Vector3 actualScale = moveBlocks[i].rectTransform.localScale;
            moveBlocks[i].LerpRectTransform(actualPosition, actualScale, moveLerpDuration);
        }

        AudioController.Instance.uiSfxSounds.Play("NewMove2");
    }
}
