using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class MySelectable
{
    protected Color32 ACTIVE_COLOR = new(255, 255, 255, 255);
    protected Color32 INACTIVE_COLOR = new(255, 255, 255, 100);

    [SerializeField] protected Toggle dependency;
    [SerializeField] protected Button button;

    public virtual void Initialize()
    {
        if (dependency != null) SetDependency();
        else SetButtonAction();
    }

    public abstract void AddListener();

    public abstract void RemoveListener();

    public abstract void SetButtonAction();

    public abstract void SetDependency();

    public virtual void ChangeColor(Color32 color)
    {
        ColorBlock cb_button = button.colors;
        cb_button.normalColor = color;
        button.colors = cb_button;
    }
}
