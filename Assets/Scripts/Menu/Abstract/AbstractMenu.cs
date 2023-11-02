using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;
using LerpUtilities;
using System.Threading.Tasks;

public abstract class AbstractMenu : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] protected float positionOffset;
    [SerializeField] [Range(0f, 1f)] protected float alphaOffset;
    [SerializeField] [Range(0f,1f)] protected float transitionTime;

    [Header("Requirements")]
    [SerializeField] protected GameObject firstSelected;

    protected List<ITransition> newTransitions = new();
    protected readonly List<MySelectable> elements = new();

    private CanvasGroup canvasGroup;
    private float initialPosition;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = transform.localPosition.x;
    }

    private void OnEnable()
    {
        EnableTransition();
    }

    protected virtual void OnDisable()
    {
        if (EventSystem.current != null && GameManager.Input.NeedToSelect)
            firstSelected = EventSystem.current.currentSelectedGameObject;

    }

    private async void EnableTransition()
    {
        await Task.WhenAll(Lerp.Value_Unscaled(transform.localPosition.x - positionOffset, initialPosition,
                                               (f) => transform.localPosition = new Vector3(f, transform.localPosition.y, transform.localPosition.z), transitionTime),
                           Lerp.Value_Unscaled(1f - alphaOffset, 1f, (f) => canvasGroup.alpha = f, transitionTime));
    }

    private void ObtainByReflection()
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (FieldInfo field in fields)
        {
            if (typeof(MySelectable).IsAssignableFrom(field.FieldType))
            {
                elements.Add((MySelectable)field.GetValue(this));
            }

            if (typeof(ITransition).IsAssignableFrom(field.FieldType))
            {
                newTransitions.Add((ITransition)field.GetValue(this));
            }
        }
    }

    public void Initialize()
    {
        if (GameManager.Input.NeedToSelect)
            EventSystem.current.SetSelectedGameObject(firstSelected);

        GameManager.Input.SetSelectedGameObject(firstSelected);
    }

    private void Start()
    {
        ObtainByReflection();
        elements.ForEach(element => element.Initialize());

        Configure();
    }

    public bool HasTransition()
    {
        foreach (ITransition transition in newTransitions)
            if (transition.HasTransition()) return true;

        return false;
    }

    protected abstract void Configure();

    protected void Slider(ref float save, float value, bool hasSound)
    {
        save = value;
        if(hasSound) GameManager.Audio.Slider();
        GameManager.Save.ApplyChanges();
    }

    protected void Toggle(ref bool save, bool value)
    {
        save = value;
        GameManager.Audio.Play("PressButton");
        GameManager.Save.ApplyChanges();
    }

    protected void Dropdown(ref string save, string value)
    {
        save = value;
        GameManager.Audio.Play("PressButton");
        GameManager.Save.ApplyChanges();
    }

    protected void Dropdown(ref int save, int value)
    {
        save = value;
        GameManager.Audio.Play("PressButton");
        GameManager.Save.ApplyChanges();
    }
}
