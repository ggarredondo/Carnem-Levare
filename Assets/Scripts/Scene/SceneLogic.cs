using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLogic", menuName = "Scriptable Objects/Scene Logic")]
public class SceneLogic : ScriptableObject
{
    [System.Serializable]
    public struct SceneData
    {
        public Object sceneObject;
        public bool withLoadScreen;
        public Object loadSceneObject;
    }

    [Header ("Scene")]
    public Object sceneObject;
    public SceneData nextScene;
    public SceneData previousScene;

    [Header("Sound")]
    public bool playMusic;
    [ConditionalField("playMusic")] public string music;
    public List<Sounds> sounds;
}
