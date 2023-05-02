using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public InputReader input;
    public BehaviourTree tree;
    public List<GameObject> menus;

    public event System.Action<int> OnSiblingChange;

    private void OnEnable()
    {
        tree.OnChange += ApplyChanges;
        input.ChangeRightMenuEvent += MoveRight;
        input.ChangeLeftMenuEvent += MoveLeft;
        input.MenuBackEvent += Return;
    }

    private void OnDisable()
    {
        tree.OnChange -= ApplyChanges;
        input.ChangeRightMenuEvent -= MoveRight;
        input.ChangeLeftMenuEvent -= MoveLeft;
        input.MenuBackEvent -= Return;
    }

    private void Start()
    {
        tree.Initialize();
    }

    public void ChangeMenu()
    {
        tree.GoToChild();
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
    }

    public void ChangeSibling(int child)
    {
        tree.ChangeSibling(child);
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        OnSiblingChange.Invoke(tree.ActualSelectableID());
    }

    public void MoveRight()
    {
        tree.MoveToRightSibling();
        GameManager.ControllerRumble.Rumble(0.1f, 0f, 1f);
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        OnSiblingChange.Invoke(tree.ActualSelectableID());
    }

    public void MoveLeft()
    {
        tree.MoveToLeftSibling();
        GameManager.ControllerRumble.Rumble(0.1f, 1f, 0f);
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
        OnSiblingChange.Invoke(tree.ActualSelectableID());
    }

    public void Return()
    {
        if (menus[tree.CurrentId()].GetComponent<AbstractMenu>().HasTransition())
            menus[tree.CurrentId()].GetComponent<AbstractMenu>().Return();
        else
        {
            tree.GoToParent();
            AudioManager.Instance.uiSfxSounds.Play("PressButton");
        }
    }

    private void ApplyChanges()
    {
        menus.ForEach(menu => menu.SetActive(false));
        tree.GetSelected().ForEach(id => { menus[id].SetActive(true); });
    }
}
