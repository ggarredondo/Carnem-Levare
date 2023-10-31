using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[NodeRelevance(typeof(DialogueTree))]
public class StandardLine : DecoratorNode, IHaveText
{
    [HideInInspector] public string text;

    [Header("Text Settings")]
    public int fontSize;
    public Color color;
    public TMP_ColorGradient colorGradient;
    public FontStyles style;
    public float characterSpacing;
    public float wordSpacing;

    [Header("Speed")]
    [Range(0f, 0.4f)] public float timeBetweenChars;

    [Header("Sound")]
    public List<string> soundsName;
    public SoundType soundGenerationType;

    [Header("Effecs")]
    public int effectDistance;

    public string Text => text;

    public int FontSize => fontSize;

    public Color Color => color;

    public FontStyles Style => style;

    public float CharacterSpacing => characterSpacing;

    public float WordSpacing => wordSpacing;

    public TMP_ColorGradient ColorGradient => colorGradient;

    public float TimeBetweenChars => timeBetweenChars;

    public List<string> SoundsName => soundsName;

    public SoundType SoundGenerationType => soundGenerationType;

    public int EffectDistance => effectDistance;

    public override void Clone(Node node)
    {
        base.Clone(node);

        fontSize = ((StandardLine)node).fontSize;
        color = ((StandardLine)node).color;
        style = ((StandardLine)node).style;
        characterSpacing = ((StandardLine)node).characterSpacing;
        wordSpacing = ((StandardLine)node).wordSpacing;
        colorGradient = ((StandardLine)node).colorGradient;

        timeBetweenChars = ((StandardLine)node).timeBetweenChars;
        effectDistance = ((StandardLine)node).effectDistance;
        soundsName = new List<string>(((StandardLine)node).soundsName);
        soundGenerationType = ((StandardLine)node).soundGenerationType;
    }
}
