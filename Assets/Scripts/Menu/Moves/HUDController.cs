using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private List<GameObject> HUDMenus;

    private int actualHUDMenu = 0;

    private void OnEnable()
    {
        inputReader.SelectMenuEvent += ChangeHUDMenu;
    }

    private void OnDisable()
    {
        inputReader.SelectMenuEvent -= ChangeHUDMenu;
    }

    public void ChangeHUDMenu()
    {
        HUDMenus.ForEach(m => m.SetActive(false));
        actualHUDMenu = (actualHUDMenu + 1) % HUDMenus.Count;
        HUDMenus[actualHUDMenu].SetActive(true);
    }
}
