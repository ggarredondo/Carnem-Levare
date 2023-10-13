using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualData", menuName = "Scriptable Objects/Visuals/VisualData")]
public class VisualData : ScriptableObject
{
    public enum QualityData
    {
        Low,
        Medium,
        High,
        Ultra
    }

    [System.Serializable]
    public struct ShadowResolutionStruct
    {
        public UnityEngine.Rendering.Universal.ShadowResolution shadowResolution;
        public int tierLow;
        public int tierMid;
        public int tierHigh;
    }

    [SerializeField] private List<Tuple<QualityData, int>> shadowDistanceList;
    [SerializeField] private List<Tuple<QualityData, ShadowResolutionStruct>> shadowResolutionList;
    [SerializeField] private List<Tuple<QualityData, int>> antialiasingList;
    [SerializeField] private List<Tuple<QualityData, int>> textureResolutionList;

    public Dictionary<int, int> shadowDistanceMap = new();
    public Dictionary<int, ShadowResolutionStruct> shadowResolutionMap = new();
    public Dictionary<int, int> antialiasingMap = new();
    public Dictionary<int, int> textureResolutionMap = new();

    public void Initialize()
    {
        foreach (Tuple<QualityData, int> tuple in shadowDistanceList)
            shadowDistanceMap.Add((int)tuple.Item1, tuple.Item2);

        foreach (Tuple<QualityData, ShadowResolutionStruct> tuple in shadowResolutionList)
            shadowResolutionMap.Add((int)tuple.Item1, tuple.Item2);

        foreach (Tuple<QualityData, int> tuple in antialiasingList)
            antialiasingMap.Add((int)tuple.Item1, tuple.Item2);

        foreach (Tuple<QualityData, int> tuple in textureResolutionList)
            textureResolutionMap.Add((int)tuple.Item1, tuple.Item2);
    }
}
