using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class SelectableMenuManager : MenuManager
{
    [SerializeField] private Button[] selectableButtons;
    [SerializeField] private Button returnButton;

    private void OnEnable()
    {
        actualActiveMenu = firstMenu;
        SetUpButtons(actualActiveMenu);
        SetActiveMenuById(actualActiveMenu, true);
    }

    /// <summary>
    /// Save the last selected button
    /// </summary>
    private void OnDisable()
    {
        if (EventSystem.current != null)
        {
            menus[actualActiveMenu].setFirstButton(EventSystem.current.currentSelectedGameObject);
            firstMenu = actualActiveMenu;
        }
    }

    /// <summary>
    /// Move to the Menu on the right when pressing right shoulder
    /// </summary>
    /// <param name="context"></param>
    public void MoveToRightMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CleanOldButtons(actualActiveMenu);
            int newActualMenu = Mod(actualActiveMenu + 1, selectableButtons.Length);
            SetUpButtons(newActualMenu);
            ChangeMenu(newActualMenu);
        }
    }

    /// <summary>
    /// Move to the Menu on the left when pressing left shoulder
    /// </summary>
    /// <param name="context"></param>
    public void MoveToLeftMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CleanOldButtons(actualActiveMenu);
            int newActualMenu = Mod(actualActiveMenu - 1, selectableButtons.Length);
            SetUpButtons(newActualMenu);
            ChangeMenu(newActualMenu);
        }
    }

    /// <summary>
    /// Selecting the menu with the mouse cursor
    /// </summary>
    /// <param name="newActualMenu">The index of the new menu</param>
    public void SelectWithMouse(int newActualMenu)
    {
        CleanOldButtons(actualActiveMenu);
        SetUpButtons(newActualMenu);
        SetActiveMenuById(newActualMenu, true);
    }

    /// <summary>
    /// Deselect the old menu
    /// </summary>
    /// <param name="oldMenu">The index of the old menu</param>
    private void CleanOldButtons(int oldMenu)
    {
        //No Highlight the old selected button
        selectableButtons[oldMenu].OnDeselect(null);
    }

    /// <summary>
    /// Set up the selectable and permanent buttons
    /// </summary>
    /// <param name="actualMenu"></param>
    private void SetUpButtons(int actualMenu)
    {
        //Highlight the actual selected button
        selectableButtons[actualMenu].OnSelect(null);
        
        //Get the Navigation data of return button
        Navigation navigation = returnButton.navigation;
        Transform buttons = menus[actualMenu].GetButtonsGameObject().transform;

        //Asign the navigation buttons
        if (buttons.childCount > 0)
        {
            navigation.selectOnDown = buttons.GetChild(0).GetComponent<Button>();
            navigation.selectOnUp = buttons.GetChild(buttons.childCount - 1).GetComponent<Button>();
        }

        //Reassign the struct data to the button
        returnButton.navigation = navigation;

    }
}
