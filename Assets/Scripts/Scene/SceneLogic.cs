using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLogic", menuName = "Scriptable Objects/Scene Logic")]
public class SceneLogic : ScriptableObject
{
    [System.Serializable]
    public struct SceneData
    {
        public string sceneName;
        public bool withLoadScreen;
        public string loadSceneName;
    }

    [Header ("Scene")]
    public string sceneName;
    public SceneData nextScene;
    public SceneData previousScene;

    [Header("Sound")]
    public bool playMusic;
    [ConditionalField("playMusic")] public string music;
    public List<SoundStructure> sounds;
}
