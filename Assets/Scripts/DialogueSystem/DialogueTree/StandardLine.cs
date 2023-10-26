using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeRelevance(typeof(DialogueTree))]
public class StandardLine : DecoratorNode
{
    public enum SoundType
    {
        BY_CHARACTER,
        BY_WORD
    }

    [System.Serializable]
    public struct SpecialCharacter
    {
        public char character;
        [Range(-100f, 1000f)] public float addition;
    }

    [Header("Text Settings")]
    public string text;

    [Header("Speed")]
    [Range(0f, 0.2f)] public float timeBetweenChars;

    [Header("Sound")]
    public List<string> soundsName;
    public SoundType soundType;

    [Header("Effecs")]
    public int effectDistance;
}
