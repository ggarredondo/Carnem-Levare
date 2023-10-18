using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class MySelectable
{
    [SerializeField] protected Button button;

    public abstract void Initialize();

    public abstract void AddListener();

    public abstract void RemoveListener();

    public virtual void ChangeColor(Color32 color)
    {
        ColorBlock cb_button = button.colors;
        cb_button.normalColor = color;
        button.colors = cb_button;
    }
}
