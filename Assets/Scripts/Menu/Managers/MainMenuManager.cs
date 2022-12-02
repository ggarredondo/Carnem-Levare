using System.Collections;
using UnityEngine;
using TMPro;

public class MainMenuManager : MenuManager
{
    private const float POP_UP_TIME = 1f;
    [SerializeField] protected GameObject popUpMenu;

    public void PopUpMessage(string message)
    {
        popUpMenu.GetComponentInChildren<TMP_Text>().text = message;
        popUpMenu.SetActive(true);
    }

    public IEnumerator PopUpForTime(string message)
    {
        PopUpMessage(message);
        yield return new WaitForSeconds(POP_UP_TIME);
        DisablePopUpMenu();
    }

    public void DisablePopUpMenu()
    {
        popUpMenu.SetActive(false);
    }
}
