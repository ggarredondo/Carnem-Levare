using UnityEngine;
using TMPro;
using LerpUtilities;

public class HoldText : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    [SerializeField] private TMP_Text tmpText;

    private void OnEnable()
    {
        inputReader.StartHoldInitEvent += StartHolding;
        inputReader.StartHoldEvent += TriggerHold;
        inputReader.StartHoldReleaseEvent += ReleaseHold;
    }

    private void OnDisable()
    {
        inputReader.StartHoldInitEvent -= StartHolding;
        inputReader.StartHoldEvent -= TriggerHold;
        inputReader.StartHoldReleaseEvent -= ReleaseHold;
    }

    private void ReleaseHold()
    {
        tmpText.color = new Color(1, 1, 1);
    }

    private async void StartHolding()
    {
        await Lerp.Text_Color(tmpText, new Color(1, 0, 0), 0.8f);
    }

    private void TriggerHold()
    {
        GameManager.SceneController.NextScene();
    }
}
