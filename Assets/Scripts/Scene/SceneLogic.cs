using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLogic", menuName = "Scriptable Objects/Scene Logic")]
public class SceneLogic : ScriptableObject
{
    [System.Serializable]
    public struct Scene
    {
        public bool withLoadScreen;
        public SceneNumber ID;
    }

    [Header ("Scene")]
    public SceneNumber ID;
    public Scene nextScene;
    public Scene previousScene;

    [Header("Sound")]
    public bool playMusic;
    [ConditionalField("playMusic")] public string music;
    public List<Sounds> sounds;
}
