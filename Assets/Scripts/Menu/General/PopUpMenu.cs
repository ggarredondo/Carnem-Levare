using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpMenu : MonoBehaviour
{
    private const float POP_UP_TIME = 1f;

    public void PopUpMessage(string message)
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = message;
        gameObject.SetActive(true);
    }

    public IEnumerator PopUpForTime(string message)
    {
        PopUpMessage(message);
        yield return new WaitForSeconds(POP_UP_TIME);
        DisablePopUpMenu();
    }

    public void DisablePopUpMenu()
    {
        gameObject.SetActive(false);
    }
}
