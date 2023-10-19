using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [System.Serializable]
    public struct Menu
    {
        public GameObject UI;
        public int logicIndex;
    }

    public InputReader input;
    public MenuTree tree;
    public List<Menu> menus;
    public Dictionary<int, GameObject> menuDictionary = new();

    [System.NonSerialized] public bool pauseMenu;

    public event System.Action<int> OnSiblingChange;
    public event System.Action ExitPauseMenuEvent;

    private void Awake()
    {
        menus.ForEach(m => menuDictionary.Add(m.logicIndex, m.UI));

        tree.OnChange += ApplyChanges;
        input.ChangeRightMenuEvent += MoveRight;
        input.ChangeLeftMenuEvent += MoveLeft;
        input.MenuBackEvent += Return;
    }

    private void OnDestroy()
    {
        tree.OnChange -= ApplyChanges;
        input.ChangeRightMenuEvent -= MoveRight;
        input.ChangeLeftMenuEvent -= MoveLeft;
        input.MenuBackEvent -= Return;
    }

    private void Start()
    {
        if(!pauseMenu)
            tree.Initialize();
    }

    public void ChangeMenu()
    {
        tree.GoToChild();
        GameManager.Audio.Play("PressButton");
    }

    public void ChangeSibling(int child)
    {
        if (tree.ChangeSibling(child))
        {
            GameManager.Audio.Play("PressButton");
            OnSiblingChange.Invoke(tree.ActualSelectableID());
        }
    }

    public void MoveRight()
    {
        if (tree.MoveToRightSibling())
        {
            GameManager.Input.Rumble(0.1f, 0f, 1f);
            GameManager.Audio.Play("PressButton");
            OnSiblingChange.Invoke(tree.ActualSelectableID());
        }
    }

    public void MoveLeft()
    {
        if (tree.MoveToLeftSibling())
        {
            GameManager.Input.Rumble(0.1f, 1f, 0f);
            GameManager.Audio.Play("PressButton");
            OnSiblingChange.Invoke(tree.ActualSelectableID());
        }
    }

    private void GoToParent()
    {
        if(tree.GoToParent())
            GameManager.Audio.Play("PressButton");
        else if(pauseMenu)
            ExitPauseMenuEvent.Invoke();
    }

    public void Return()
    {
        AbstractMenu abs = menuDictionary[tree.CurrentId()].GetComponent<AbstractMenu>();

        if(!abs.HasTransition()) GoToParent();
    }

    private void ApplyChanges()
    {
        DisableMenus();
        tree.GetSelected().ForEach(id => { menuDictionary[id].GetComponent<AbstractMenu>()?.Initialized(); menuDictionary[id].SetActive(true); });
    }

    public void DisableMenus()
    {
        menus.ForEach(menu => menu.UI.SetActive(false));
    }
}
