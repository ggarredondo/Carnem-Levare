using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeRelevance(typeof(DialogueTree))]
public class LeafLine : LeafNode, IHaveText
{
    [HideInInspector] public string text;

    [Header("Text Settings")]
    public int fontSize;

    [Header("Speed Settings")]
    [Range(0f, 0.2f)] public float timeBetweenChars;

    [Header("Sound")]
    public List<string> soundsName;

    [Header("Effecs")]
    public int effectDistance;

    public string Text => text;

    public int FontSize => fontSize;

    public float TimeBetweenChars => timeBetweenChars;

    public List<string> SoundsName => soundsName;

    public int EffectDistance => effectDistance;
}
