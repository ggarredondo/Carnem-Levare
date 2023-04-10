using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public abstract class MenuManager : MonoBehaviour
{
    [Header ("Menu Assets")]
    [SerializeField] protected InputReader inputReader = default;
    [SerializeField] protected Menu[] menus;
    [SerializeField] protected int firstMenu;

    protected int actualActiveMenu;

    protected virtual void OnEnable()
    {
        inputReader.MouseClickEvent += MouseClick;
        inputReader.MenuBackEvent += ReturnFromChildren;
    }

    protected virtual void OnDisable()
    {
        inputReader.MouseClickEvent -= MouseClick;
        inputReader.MenuBackEvent -= ReturnFromChildren;
    }

    protected virtual void Awake()
    {
        actualActiveMenu = firstMenu;
    }

    private void Start()
    {
        SetActiveMenuById(actualActiveMenu, true);
    }

    #region Public

    /// <summary>
    /// Return to the parent menu of the actual active menu
    /// </summary>
    public void ReturnToParent()
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");

        if (menus[actualActiveMenu].GetParentName() != menus[actualActiveMenu].GetName())
        {
            int parentId = GetIdByName(menus[actualActiveMenu].GetParentName());
            SetActiveMenuById(parentId, true);
        }
    }

    /// <summary>
    /// Change the actual menu
    /// </summary>
    public void ChangeMenu(int id)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");

        if (EventSystem.current.currentSelectedGameObject != null)
            menus[actualActiveMenu].SetFirstButton(EventSystem.current.currentSelectedGameObject);

        SetActiveMenuById(id, true);
    }

    /// <summary>
    /// Change the actual menuManager
    /// </summary>
    public void ChangeMenuNoInitialize(int id)
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");

        if (EventSystem.current.currentSelectedGameObject != null)
            menus[actualActiveMenu].SetFirstButton(EventSystem.current.currentSelectedGameObject);

        SetActiveMenuById(id, false);
    }

    /// <summary>
    /// Go to a children of the actual object
    /// </summary>
    public void GoToChildren()
    {
        AudioManager.Instance.uiSfxSounds.Play("PressButton");

        GameObject children = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject;
        EventSystem.current.SetSelectedGameObject(children);
    }

    #endregion

    #region Private&Protected

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
        {
            ControlSaver.Instance.selected = menus[actualActiveMenu].GetFirstButton();

            if (ControlSaver.Instance.controlSchemeIndex == 0)
            {
                EventSystem.current.SetSelectedGameObject(menus[actualActiveMenu].GetFirstButton());
                AudioManager.Instance?.uiSfxSounds.Stop("SelectButton");
            }
        }

        menus[actualActiveMenu].SetActive(true);
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
    /// Disable the actual active menu, used by the pause menu
    /// </summary>
    protected void DisableActiveMenu()
    {
        menus[actualActiveMenu].SetActive(false);
    }

    /// <summary>
    /// Raycast the UI elements behind the mouse position
    /// </summary>
    /// <returns>List of elements sort by depth</returns>
    private List<RaycastResult> RaycastMouse()
    {
        PointerEventData pointerData = new(EventSystem.current) { pointerId = -1, };
        List<RaycastResult> results = new();

        pointerData.position = Mouse.current.position.ReadValue();
        EventSystem.current.RaycastAll(pointerData, results);

        return results;
    }

    /// <summary>
    /// Return to the parent of the actual object
    /// </summary>
    protected virtual void ReturnFromChildren()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current != null)
        {
            if (current.GetComponent<Button>() == null)
            {
                if (current.GetComponent<TMP_Dropdown>() != null)
                {
                    current.GetComponent<TMP_Dropdown>().Hide();
                }

                GameObject Parent = current.transform.parent.gameObject;
                EventSystem.current.SetSelectedGameObject(Parent);

                if (Parent.GetComponent<Button>() == null)
                    ReturnFromChildren();
            }
            else
            {
                ReturnToParent();
            }
        }
        else
        {
            ReturnToParent();
        } 
    }

    /// <summary>
    /// Obtain the elements behind the mouse and if there is a slider it will set as the selected one
    /// </summary>
    private void MouseClick()
    {
        if (RaycastMouse().Count != 0)
            if (RaycastMouse()[0].gameObject.name == "Handle")
                EventSystem.current.SetSelectedGameObject(RaycastMouse()[0].gameObject.transform.parent.parent.gameObject);
    }

    /// <summary>
    /// Math module operation, the c# on unity not working as expected
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    protected int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }

    #endregion
}
