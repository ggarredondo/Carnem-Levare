using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public InputReader input;
    public BehaviourTree tree;
    public List<GameObject> menus;

    public bool pauseMenu;

    public event System.Action<int> OnSiblingChange;

    private void Awake()
    {
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
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
    }

    public void ChangeSibling(int child)
    {
        if (tree.ChangeSibling(child))
        {
            AudioManager.Instance.uiSfxSounds.Play("PressButton");
            OnSiblingChange.Invoke(tree.ActualSelectableID());
        }
    }

    public void MoveRight()
    {
        if (tree.MoveToRightSibling())
        {
            GameManager.ControllerRumble.Rumble(0.1f, 0f, 1f);
            AudioManager.Instance.uiSfxSounds.Play("PressButton");
            OnSiblingChange.Invoke(tree.ActualSelectableID());
        }
    }

    public void MoveLeft()
    {
        if (tree.MoveToLeftSibling())
        {
            GameManager.ControllerRumble.Rumble(0.1f, 1f, 0f);
            AudioManager.Instance.uiSfxSounds.Play("PressButton");
            OnSiblingChange.Invoke(tree.ActualSelectableID());
        }
    }

    public void GoToParent()
    {
        tree.GoToParent();
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
    }

    public void Return()
    {
        AbstractMenu abs = menus[tree.CurrentId()].GetComponent<AbstractMenu>();

        if(!abs.Return())
        {
            GoToParent();
        } 
    }

    private void ApplyChanges()
    {
        DisableMenus();
        tree.GetSelected().ForEach(id => { menus[id].GetComponent<AbstractMenu>()?.Initialized(); menus[id].SetActive(true); });
    }

    public void DisableMenus()
    {
        menus.ForEach(menu => menu.SetActive(false));
    }
}
