using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TreeUtilities;

namespace DialogueTreeUtilities
{
    [NodeRelevance(typeof(DialogueTree))]
    public class LeafLine : LeafNode, IHaveText
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

            fontSize = ((LeafLine)node).fontSize;
            color = ((LeafLine)node).color;
            style = ((LeafLine)node).style;
            characterSpacing = ((LeafLine)node).characterSpacing;
            wordSpacing = ((LeafLine)node).wordSpacing;
            colorGradient = ((LeafLine)node).colorGradient;

            timeBetweenChars = ((LeafLine)node).timeBetweenChars;
            effectDistance = ((LeafLine)node).effectDistance;
            soundsName = new List<string>(((LeafLine)node).soundsName);
            soundGenerationType = ((LeafLine)node).soundGenerationType;
        }
    }
}
