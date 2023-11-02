using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LerpUtilities;

public class SelectMenu : MonoBehaviour
{
    [SerializeField] private MenuController menuController;

    [Header("UI Elements")]
    [SerializeField] private List<Button> selectableButton;

    protected void OnEnable()
    {
        Select(menuController.tree.ActualSelectableID());
    }

    private void Start()
    {
        Configure();
    }

    protected void Configure()
    {
        menuController.OnSiblingChange += Select;
    }

    private void Select(int button)
    {
        for (int i = 0; i < selectableButton.Count; i++)
        {
            if (i == button)
                selectableButton[i].OnSelect(null);
            else
                selectableButton[i].OnDeselect(null);
        }
    }
}
