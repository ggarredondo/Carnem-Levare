using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "VisualOptions", menuName = "Scriptable Objects/Visuals/VisualOptions")]
public class VisualOptionsApplier : ScriptableObject
{
    [SerializeField] private List<MyURPAsset> urpQuality;
    [SerializeField] private MyURPAsset urpCustom;

    public void Initialize()
    {
        foreach(MyURPAsset asset in urpQuality)
        {
            asset.ApplyChanges();
        }

        ApplyChanges();
    }

    public void ApplyChanges()
    {
        urpCustom.ApplyChanges();
    }

    public int CustomIndex { get => urpCustom.ID;}

    public MyURPAsset GetCustomQuality()
    {
        return urpCustom;
    }

    public MyURPAsset GetQuality(int value)
    {
        if(value == urpCustom.ID)
            return urpCustom;
        else
            return urpQuality[value];
    }
}
