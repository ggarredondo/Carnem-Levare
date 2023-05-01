using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public InputReader input;
    public BehaviourTree tree;
    public List<GameObject> menus;

    private void OnEnable()
    {
        tree.OnChange += ApplyChanges;
        input.ChangeRightMenuEvent += tree.MoveToRightSibling;
        input.ChangeLeftMenuEvent += tree.MoveToLeftSibling;
        input.MenuBackEvent += Return;
    }

    private void OnDisable()
    {
        tree.OnChange -= ApplyChanges;
        input.ChangeRightMenuEvent -= tree.MoveToRightSibling;
        input.ChangeLeftMenuEvent -= tree.MoveToLeftSibling;
        input.MenuBackEvent -= Return;
    }

    private void Start()
    {
        tree.Initialize();
    }

    public void ChangeMenu()
    {
        tree.GoToChild();
    }

    public void Return()
    {
        if(menus[tree.CurrentId()].GetComponent<AbstractMenu>().HasTransition())
            menus[tree.CurrentId()].GetComponent<AbstractMenu>().Return();
        else
            tree.GoToParent();
    }

    private void ApplyChanges()
    {
        menus.ForEach(menu => menu.SetActive(false));
        tree.GetSelected().ForEach(id => menus[id].SetActive(true) );
        AudioManager.Instance.uiSfxSounds.Play("PressButton");
    }
}
