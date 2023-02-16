using UnityEngine;

[System.Serializable]
public class Menu
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject parentMenu;
    [SerializeField] private GameObject firstSelectedButton;

    public void SetActive(bool active)
    {
        menu.SetActive(active);
    }
    
    public bool IsActive()
    {
        return menu.activeSelf;
    }

    public string GetName()
    {
        return menu.name;
    }

    public GameObject GetButtonsGameObject()
    {
        RectTransform buttons = menu.transform.GetChild(0).GetComponent<RectTransform>();
        return buttons.gameObject;
    }

    public string GetParentName()
    {
        return parentMenu.name;
    }

    public GameObject GetFirstButton()
    {
        return firstSelectedButton;
    }

    public void SetFirstButton(GameObject firstButton)
    {
        firstSelectedButton = firstButton;
    }
}
