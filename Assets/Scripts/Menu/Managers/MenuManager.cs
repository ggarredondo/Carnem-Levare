using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] protected Menu[] menus;
    [SerializeField] protected int firstMenu;
    [SerializeField] private GameObject popUpMenu;

    protected int actualActiveMenu;

    private void Awake()
    {
        actualActiveMenu = firstMenu;
        SetActiveMenuById(actualActiveMenu, true);
    }

    public void PopUpMessage(string message)
    {
        popUpMenu.GetComponentInChildren<TMP_Text>().text = message;
        popUpMenu.SetActive(true);
    }

    public void DisablePopUpMenu()
    {
        popUpMenu.SetActive(false);
    }

    /// <summary>
    /// Set the active menu by the id of the 'menus' Array
    /// </summary>
    /// <param name="id">The menus array index</param>
    /// <param name="activeFirstButton">If want to set an active button by default</param>
    protected void SetActiveMenuById(int id, bool activeFirstButton)
    {
        menus[actualActiveMenu].SetActive(false);

        actualActiveMenu = id;

        if (activeFirstButton)
            EventSystem.current.SetSelectedGameObject(menus[actualActiveMenu].GetFirstButton());

        menus[actualActiveMenu].SetActive(true);
    }

    /// <summary>
    /// Disable the actual active menu, used by the pause menu
    /// </summary>
    protected void DisableActiveMenu()
    {
        menus[actualActiveMenu].SetActive(false);
    }

    /// <summary>
    /// Return to the parent menu of the actual active menu
    /// </summary>
    public void ReturnToParent()
    {
        if (menus[actualActiveMenu].GetParentName() != menus[actualActiveMenu].GetName())
        {
            int parentId = GetIdByName(menus[actualActiveMenu].GetParentName());
            SetActiveMenuById(parentId, true);
        }
    } 

    /// <summary>
    /// Get the index of the menus array by the menu name
    /// </summary>
    /// <param name="name">The menu name</param>
    /// <returns></returns>
    private int GetIdByName(string name)
    {
        int tmp = 0;

        for (int i = 0; i < menus.Length; i++)
            if (menus[i].GetName() == name)
                tmp = i;

        return tmp;
    }

    /// <summary>
    /// Change the actual menu
    /// </summary>
    public void ChangeMenu(int id)
    {
        if(EventSystem.current.currentSelectedGameObject != null)
            menus[actualActiveMenu].setFirstButton(EventSystem.current.currentSelectedGameObject);

        SetActiveMenuById(id, true);
    }

    /// <summary>
    /// Change the actual menuManager
    /// </summary>
    public void ChangeMenuNoInitialize(int id)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            menus[actualActiveMenu].setFirstButton(EventSystem.current.currentSelectedGameObject);

        SetActiveMenuById(id, false);
    }

    /// <summary>
    /// Raycast the UI elements behind the mouse position
    /// </summary>
    /// <returns>List of elements sort by depth</returns>
    private List<RaycastResult> RaycastMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { pointerId = -1, };
        List<RaycastResult> results = new List<RaycastResult>();

        pointerData.position = Mouse.current.position.ReadValue();
        EventSystem.current.RaycastAll(pointerData, results);

        return results;
    }

    /// <summary>
    /// Go to a children of the actual object
    /// </summary>
    public void GoToChildren()
    {
        GameObject children = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject;
        EventSystem.current.SetSelectedGameObject(children);
    }

    /// <summary>
    /// Return to the parent of the actual object
    /// </summary>
    /// <param name="context"></param>
    public void ReturnFromChildren(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;

            if (current.GetComponent<Button>() == null)
            {
                if (current.GetComponent<TMP_Dropdown>() != null)
                {
                    current.GetComponent<TMP_Dropdown>().Hide();
                }

                GameObject Parent = current.transform.parent.gameObject;
                EventSystem.current.SetSelectedGameObject(Parent);

                if (Parent.GetComponent<Button>() == null)
                    ReturnFromChildren(context);

            }
            else
            {
                ReturnToParent();
            }
        }

    }

    /// <summary>
    /// Obtain the elements behind the mouse and if there is a slider it will set as the selected one
    /// </summary>
    /// <param name="context"></param>
    public void MouseClick(InputAction.CallbackContext context)
    {
        if (RaycastMouse().Count != 0)
            if (RaycastMouse()[0].gameObject.name == "Handle")
                EventSystem.current.SetSelectedGameObject(RaycastMouse()[0].gameObject.transform.parent.parent.gameObject);
    }
}